﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using System;

public class KinectDataClient : MonoBehaviour 
{
	[Tooltip("Port used for server broadcast discovery.")]
	public int broadcastPort = 8889;

	[Tooltip("Host name or IP, where Kinect data server is runing.")]
	public string serverHost = "0.0.0.0";

	[Tooltip("Port, on which Kinect data server is listening.")]
	public int serverPort = 8888;

	[Tooltip("Port, on which the client is connecting.")]
	public int clientPort = 8887;

	[Tooltip("Try to reconnect after this amount of seconds.")]
	public float reconnectAfter = 2f;

	[Tooltip("GUI-Text to display status messages.")]
	public TextMesh statusText;

	// whether the client is connected to the server
	private bool connected = false;
	//private bool connectedOnce = false;
	private float disconnectedAt = 0f;

	private ConnectionConfig clientConfig;
	private int clientChannelId;

	private HostTopology clientTopology;
	private int clientHostId = -1;
	private int clientConnId = -1;
	private int bcastHostId = -1;

	private const int bufferSize = 32768;
	private byte[] recBuffer = new byte[bufferSize];
	private byte[] bcastBuffer = new byte[1024];
	private float dataReceivedAt = 0f;

	private bool[] sendKeepAlive = new bool[4];
	private string[] keepAliveData = new string[4];
	private int keepAliveIndex = 0, keepAliveCount = 0;


//	// buffers for face verts and tris
//	private StringBuilder sbFvBuffer = new StringBuilder ();
//	private StringBuilder sbFtBuffer = new StringBuilder ();

	private KinectManager manager;
	//private FacetrackingManager faceManager;
	private VisualGestureManager gestureManager;
	private SpeechManager speechManager;

	private byte[] compressBuffer = new byte[bufferSize];
	private LZ4Sharp.ILZ4Decompressor decompressor;
	private LZ4Sharp.ILZ4Compressor compressor;

	private static KinectDataClient instance;

    static int tmpsize = 256;
    byte[] tmp = new byte[tmpsize];

    
 


	/// <summary>
	/// Gets the singleton instance of this client.
	/// </summary>
	/// <value>The instance.</value>
	public static KinectDataClient Instance
	{
		get
		{
			return instance;
		}
	}


	/// <summary>
	/// Gets the connected-to-server status of the Kinect data client.
	/// </summary>
	/// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
	public bool IsConnected
	{
		get
		{
			return connected;
		}
	}


	// appends response message to keepAliveData[0]
	public void AddResponseMsg(string msg)
	{
		int iMsg = keepAliveData[0].IndexOf("," + msg);

		if (iMsg < 0) 
		{
			keepAliveData[0] += "," + msg;
		}
	}


	// removes response message from keepAliveData[0]
	public void RemoveResponseMsg(string msg)
	{
		int iMsg = keepAliveData[0].IndexOf(msg);

		if (iMsg >= 0) 
		{
			int iEnd = keepAliveData[0].IndexOf(',', iMsg + 1);

			if (iEnd >= 0)
				keepAliveData[0] = keepAliveData[0].Substring(0, iMsg) + keepAliveData[0].Substring(iEnd);
			else
				keepAliveData[0] = keepAliveData[0].Substring(0, iMsg);
		}
	}


    void Awake()
	{
		instance = this;
        tmp[0] = 1;
        tmp[100] = 2;
        tmp[200] = 3;


        try 
		{
			NetworkTransport.Init();

           

            clientConfig = new ConnectionConfig();
			clientChannelId = clientConfig.AddChannel(QosType.StateUpdate);  // QosType.UnreliableFragmented

			// add client host
			clientTopology = new HostTopology(clientConfig, 1);
			clientHostId = NetworkTransport.AddHost(clientTopology, clientPort);

			if(clientHostId < 0)
			{
				throw new UnityException("AddHost failed for client port " + clientPort);
			}

			if(broadcastPort > 0)
			{
				// add broadcast host
				bcastHostId = NetworkTransport.AddHost(clientTopology, broadcastPort);

				if(bcastHostId < 0)
				{
					throw new UnityException("AddHost failed for broadcast port " + broadcastPort);
				}

				// start broadcast discovery
				byte error = 0;
				NetworkTransport.SetBroadcastCredentials(bcastHostId, 8888, 1, 0, out error);
			}

			// construct keep-alive data
			keepAliveData[0] = "ka,kb,km,kh";  // index 0
			sendKeepAlive[0] = true;

//			faceManager = GetComponent<FacetrackingManager>();
//			if(faceManager != null && faceManager.isActiveAndEnabled)
//			{
//				keepAliveData[1] = "ka,fp,";  // index 1
//				sendKeepAlive[1] = true;
//
//				if(faceManager.getFaceModelData)
//				{
//					keepAliveData[2] = "ka,fv,";  // index 2
//					sendKeepAlive[2] = true;
//
//					if(faceManager.texturedModelMesh == FacetrackingManager.TextureType.FaceRectangle)
//					{
//						keepAliveData[2] += "fu,";  // request uvs
//					}
//
//					keepAliveData[3] = "ka,ft,";  // index 3
//					sendKeepAlive[3] = true;
//				}
//			}

			keepAliveCount = keepAliveData.Length;
		} 
		catch (System.Exception ex) 
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);

			if(statusText)
			{
				statusText.text = ex.Message;
			}
		}
	}


	void Start () 
	{
		// get references to the needed components
		manager = KinectManager.Instance;
		gestureManager = VisualGestureManager.Instance;
		speechManager = SpeechManager.Instance;

		// create lz4 compressor & decompressor
		decompressor = LZ4Sharp.LZ4DecompressorFactory.CreateNew();
		compressor = LZ4Sharp.LZ4CompressorFactory.CreateNew();

		try 
		{
			if(serverHost != string.Empty && serverHost != "0.0.0.0" && serverPort != 0)
			{
				byte error = 0;
				clientConnId = NetworkTransport.Connect(clientHostId, serverHost, serverPort, 0, out error);

				if(error == (byte)NetworkError.Ok)
				{
					Debug.Log("Connecting to the server - " + serverHost + ":" + serverPort);

					if(statusText)
					{
						statusText.text = "Connecting to the server...";
					}
				}
				else
				{
					throw new UnityException("Error while connecting: " + (NetworkError)error);
				}
			}
			else if(broadcastPort > 0)
			{
				Debug.Log("Waiting for the server...");

				if(statusText)
				{
					statusText.text = "Waiting for the server...";
				}
			}
			else
			{
				Debug.Log("Server settings are not set. Cannot connect.");

				if(statusText)
				{
					statusText.text = "Server settings are not set. Cannot connect.";
				}
			}
		} 
		catch (System.Exception ex) 
		{
			Debug.LogError(ex.Message + "\n" + ex.StackTrace);

			if(statusText)
			{
				statusText.text = ex.Message;
			}
		}
	}


	void OnDestroy()
	{
		if(connected)
		{
			byte error = 0;
			if(!NetworkTransport.Disconnect(clientHostId, clientConnId, out error))
			{
				Debug.Log("Error while disconnecting: " + (NetworkError)error);
			}
		}

		if (clientHostId >= 0) 
		{
			//NetworkTransport.RemoveHost (clientHostId);
			clientHostId = -1;
		}

		if (bcastHostId >= 0) 
		{
			//NetworkTransport.RemoveHost (bcastHostId);
			bcastHostId = -1;
		}

		// shitdown the transport layer
		NetworkTransport.Shutdown();
	}

	
	void Update () 
	{
		int recHostId; 
		int connectionId; 
		int recChannelId; 
		int dataSize;

       

        // enable play mode if needed
        if (manager && manager.IsInitialized() && !manager.IsPlayModeEnabled())
		{
			manager.EnablePlayMode(true);
		}

		// connect after broadcast discovery, if needed
		if (clientConnId < 0 && serverHost != string.Empty && serverHost != "0.0.0.0" && serverPort != 0) 
		{
			Start();
		}

		try 
		{
			byte error = 0;

            NetworkTransport.Send(clientHostId, clientConnId, clientChannelId, tmp, tmpsize, out error);

            // disconnect if no data received for the last 10 seconds
            if (connected && (Time.time - dataReceivedAt) >= 10f)
			{
				NetworkTransport.Disconnect(clientHostId, clientConnId, out error);
				dataReceivedAt = Time.time;

				if(error != (byte)NetworkError.Ok)
				{
					throw new UnityException("Disconnect: " + (NetworkError)error);
				}
			}

			if(connected && keepAliveIndex < keepAliveCount)
			{
				if(sendKeepAlive[keepAliveIndex] && !string.IsNullOrEmpty(keepAliveData[keepAliveIndex]))
				{
					// send keep-alive to the server
					sendKeepAlive[keepAliveIndex] = false;
					byte[] btSendMessage = System.Text.Encoding.UTF8.GetBytes(keepAliveData[keepAliveIndex]);

					int compSize = 0;
					if(compressor != null && btSendMessage.Length >= 100)
					{
						compSize = compressor.Compress(btSendMessage, 0, btSendMessage.Length, compressBuffer, 0);
					}
					else
					{
						System.Buffer.BlockCopy(btSendMessage, 0, compressBuffer, 0, btSendMessage.Length);
						compSize = btSendMessage.Length;
					}

					NetworkTransport.Send(clientHostId, clientConnId, clientChannelId, compressBuffer, compSize, out error);
					//Debug.Log(clientConnId + "-keep: " + keepAliveData[keepAliveIndex]);

					if(error != (byte)NetworkError.Ok)
					{
						throw new UnityException("Keep-alive: " + (NetworkError)error);
					}

					// make sure sr-message is sent just once
					if(keepAliveIndex == 0 && keepAliveData[0].IndexOf(",sr") >= 0)
					{
						RemoveResponseMsg(",sr");
					}
				}

				keepAliveIndex++;
				if(keepAliveIndex >= keepAliveCount)
					keepAliveIndex = 0;
			}

            // get next receive event
            NetworkEventType recData;

			if(serverHost != string.Empty && serverHost != "0.0.0.0" && serverPort != 0)
				recData = NetworkTransport.ReceiveFromHost(clientHostId, out connectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);
			else
				recData = NetworkTransport.Receive(out recHostId, out connectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);  // wait for broadcast
			
			switch (recData)
			{
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:
				if(connectionId == clientConnId)
				{
					connected = true;
					//connectedOnce = true;

					disconnectedAt = 0f;
					dataReceivedAt = Time.time;
					//sendKeepAlive = false;

					Debug.Log("Connected.");

					if(statusText)
					{
						statusText.text = "Connected.";
                            int snowlevel = 0;
                            this.GetComponent<WeatherControl2>().snowvalue = snowlevel;
                            //statusText.text = snowlevel;
                            NetworkTransport.Send(clientHostId, clientConnId, clientChannelId, compressBuffer, snowlevel, out error);

                        }
                    }
            
				break;
			case NetworkEventType.DataEvent:
				if(connectionId == clientConnId)
				{
					if(error != (byte)NetworkError.Ok)
					{
						Debug.Log("Receive error on connection " + connectionId + ": " + (NetworkError)error);
					}
					else
					{
						dataReceivedAt = Time.time;
						//sendKeepAlive = true;

						//string sRecvMessage = System.Text.Encoding.UTF8.GetString(recBuffer, 0, dataSize);
						int decompSize = 0;
						if(decompressor != null && (recBuffer[0] > 127 || recBuffer[0] < 32))
						{
							decompSize = decompressor.Decompress(recBuffer, 0, compressBuffer, 0, dataSize);
						}
						else
						{
							System.Buffer.BlockCopy(recBuffer, 0, compressBuffer, 0, dataSize);
							decompSize = dataSize;
						}

						string sRecvMessage = System.Text.Encoding.UTF8.GetString(compressBuffer, 0, decompSize);

//						if(sRecvMessage.StartsWith("pv"))
//						{
//							//Debug.Log("Got part face verts - " + sRecvMessage.Substring(0, 3));
//
//							// part of face-vertices msg
//							sRecvMessage = ProcessPvMessage(sRecvMessage);
//
//							if(sRecvMessage.Length == 0)
//								EnableNextKeepAlive(2);
//						}
//						else if(sRecvMessage.StartsWith("pt"))
//						{
//							//Debug.Log("Got part face tris - " + sRecvMessage.Substring(0, 3));
//
//							// part of face-triangles msg
//							sRecvMessage = ProcessPtMessage(sRecvMessage);
//
//							if(sRecvMessage.Length == 0)
//								EnableNextKeepAlive(3);
//						}

						if(!string.IsNullOrEmpty(sRecvMessage))
						{
							char[] msgDelim = { '|' };
							string[] asMessages = sRecvMessage.Split(msgDelim);

							char[] partDelim = { ',' };
							for(int i = 0; i < asMessages.Length; i++)
							{
								if(manager && asMessages[i].Length > 3)
								{
									if(asMessages[i].StartsWith("kb,"))
									{
										//Debug.Log("Got body data");
										manager.SetBodyFrameData(asMessages[i]);
										EnableNextKeepAlive(0);
									}
									else if(asMessages[i].StartsWith("kh,"))
									{
										manager.SetBodyHandData(asMessages[i]);
									}
									else if(asMessages[i].StartsWith("km,"))
									{
										manager.SetWorldMatrixData(asMessages[i]);
									}
									else if(asMessages[i].StartsWith("vg,") && gestureManager != null)
									{
										gestureManager.SetGestureDataFromCsv(asMessages[i], partDelim);
									}
									else if(asMessages[i].StartsWith("sr,") && speechManager != null)
									{
										speechManager.SetSpeechDataFromCsv(asMessages[i], partDelim);
									}
//									else if(asMessages[i].StartsWith("fp,") && faceManager != null)
//									{
//										//Debug.Log("Got face params");
//										faceManager.SetFaceParamsFromCsv(asMessages[i]);
//										EnableNextKeepAlive(1);
//									}
//									else if(asMessages[i].StartsWith("fv,") && faceManager != null)
//									{
//										//Debug.Log("Got face vertices");
//										faceManager.SetFaceVerticesFromCsv(asMessages[i]);
//										EnableNextKeepAlive(2);
//									}
//									else if(asMessages[i].StartsWith("fu,") && faceManager != null)
//									{
//										//Debug.Log("Got face uvs");
//										faceManager.SetFaceUvsFromCsv(asMessages[i]);
//										EnableNextKeepAlive(2);
//									}
//									else if(asMessages[i].StartsWith("ft,") && faceManager != null)
//									{
//										//Debug.Log("Got face triangles");
//										faceManager.SetFaceTrianglesFromCsv(asMessages[i]);
//
//										keepAliveData[3] = null;  // clear index 3 - one set of tris is enough
//										EnableNextKeepAlive(3);
//									}
								}
							}
						}

					}

				}
				break;
			case NetworkEventType.DisconnectEvent:
				if(connectionId == clientConnId)
				{
					connected = false;
					//connectedOnce = true;  // anyway, try to reconnect

					disconnectedAt = Time.time;
					dataReceivedAt = 0f;
					//sendKeepAlive = false;

					Debug.Log("Disconnected: " + (NetworkError)error);

					if(error != (byte)NetworkError.Ok)
					{
						throw new UnityException("Disconnected: " + (NetworkError)error);
					}
				}
				break;

			case NetworkEventType.BroadcastEvent:
				int receivedSize;
				NetworkTransport.GetBroadcastConnectionMessage(bcastHostId, bcastBuffer, bcastBuffer.Length, out receivedSize, out error);

				string senderAddr;
				int senderPort;
				NetworkTransport.GetBroadcastConnectionInfo(bcastHostId, out senderAddr, out senderPort, out error);

				if(serverHost == string.Empty || serverHost == "0.0.0.0" || serverPort == 0)
				{
					string sData = System.Text.Encoding.UTF8.GetString(bcastBuffer, 0, bcastBuffer.Length).Trim();
					OnReceivedBroadcast(senderAddr, sData);
				}
				break;
			}

			// try to reconnect, if disconnected
			if(!connected && /**connectedOnce &&*/ disconnectedAt > 0f && (Time.time - disconnectedAt) >= reconnectAfter)
			{
				disconnectedAt = 0f;

				error = 0;
				clientConnId = NetworkTransport.Connect(clientHostId, serverHost, serverPort, 0, out error);

				if(error == (byte)NetworkError.Ok)
				{
					Debug.Log("Reconnecting to the server - " + serverHost + ":" + serverPort);

					if(statusText)
					{
						statusText.text = "Reconnecting to the server...";
					}
				}
				else
				{
					throw new UnityException("Error while reconnecting: " + (NetworkError)error);
				}
			}

		}
		catch (System.Exception ex) 
		{
			//Debug.LogError(ex.Message + "\n" + ex.StackTrace);

			if(statusText)
			{
				statusText.text = ex.Message;
			}
		}
	}


	private void OnReceivedBroadcast(string fromAddress, string data)
	{
		Debug.Log (string.Format("Got broadcast from {0}: {1}", fromAddress, data));
		var items = data.Split(':');

		if (items.Length == 3 && items [0] == "KinectDataServer") 
		{
			serverHost = items [1];
			serverPort = int.Parse (items [2]);
		} 
		else if (data == string.Empty && fromAddress != string.Empty) 
		{
			items = fromAddress.Split(':');
			serverHost = items[items.Length - 1];

			if(serverPort == 0)
				serverPort = 8888;
		}

		if (serverHost != string.Empty && serverHost != "0.0.0.0" && serverPort != 0) 
		{
			// try to connect
			//connectedOnce = true;
			disconnectedAt = 0f;
		}
	}

	// enable next available keep-alive sending
	private void EnableNextKeepAlive(int currentIndex)
	{
		currentIndex = currentIndex % keepAliveCount;
		int nextIndex = (currentIndex + 1) % keepAliveCount;

		while (string.IsNullOrEmpty(keepAliveData[nextIndex]) && nextIndex != currentIndex) 
		{
			nextIndex = (nextIndex + 1) % keepAliveCount;
		}

		if (!string.IsNullOrEmpty (keepAliveData [nextIndex])) 
		{
			sendKeepAlive [nextIndex] = true;
		}
	}

    //	// processes part of face-vertices msg
    //	private string ProcessPvMessage(string sRecvMessage)
    //	{
    //		if (sRecvMessage.Length > 3) 
    //		{
    //			char chAction = sRecvMessage [2];
    //
    //			if(chAction == '0' || chAction == '3')
    //			{
    //				sbFvBuffer.Remove (0, sbFvBuffer.Length);
    //			}
    //
    //			if (chAction >= '0' && chAction <= '3') 
    //			{
    //				sbFvBuffer.Append (sRecvMessage.Substring (3));
    //			}
    //			else 
    //			{
    //				Debug.Log ("Invalid pv-rcvMsg: " + sRecvMessage);
    //			}
    //
    //			sRecvMessage = string.Empty;
    //			if(chAction == '2' || chAction == '3')
    //			{
    //				sRecvMessage = sbFvBuffer.ToString ();
    //				sbFvBuffer.Remove (0, sbFvBuffer.Length);
    //
    //				byte[] compressed = System.Convert.FromBase64String(sRecvMessage);
    //				byte[] decompressed = decompressor.Decompress(compressed);
    //				sRecvMessage = System.Text.Encoding.UTF8.GetString(decompressed);
    //			}
    //		}
    //
    //		return sRecvMessage;
    //	}
    //
    //	// processes part of face-triangles msg
    //	private string ProcessPtMessage(string sRecvMessage)
    //	{
    //		if (sRecvMessage.Length > 3) 
    //		{
    //			char chAction = sRecvMessage [2];
    //
    //			if(chAction == '0' || chAction == '3')
    //			{
    //				sbFtBuffer.Remove (0, sbFtBuffer.Length);
    //			}
    //
    //			if (chAction >= '0' && chAction <= '3') 
    //			{
    //				sbFtBuffer.Append (sRecvMessage.Substring (3));
    //			} 
    //			else 
    //			{
    //				Debug.Log ("Invalid pt-rcvMsg: " + sRecvMessage);
    //			}
    //
    //			sRecvMessage = string.Empty;
    //			if(chAction == '2' || chAction == '3')
    //			{
    //				sRecvMessage = sbFtBuffer.ToString ();
    //				sbFtBuffer.Remove (0, sbFtBuffer.Length);
    //
    //				byte[] compressed = System.Convert.FromBase64String(sRecvMessage);
    //				byte[] decompressed = decompressor.Decompress(compressed);
    //				sRecvMessage = System.Text.Encoding.UTF8.GetString(decompressed);
    //			}
    //		}
    //
    //		return sRecvMessage;
    //	}

}
