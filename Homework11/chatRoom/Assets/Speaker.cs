using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Speaker : NetworkBehaviour
{
    public GameObject itemPrefab;
    private Transform content;
    private InputField inputBox;
    private Button sendButton;
    //[SyncVar(hook = "OnValueChanged")]  
    [SyncVar]
    private int onlineNum = 0;
    void Start()
    {
        content = GameObject.Find("Canvas/Scroll View/Viewport/Content").transform;
        inputBox = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
        sendButton = GameObject.Find("Canvas/SendButton").GetComponent<Button>();
        sendButton.onClick.AddListener(SendButtonCallback);
    }
    /// <summary>  
    /// 显示在线人数  
    /// </summary>  
    private void OnGUI()
    {
        if (!isLocalPlayer)
            return;
        GUI.Label(new Rect(new Vector2(10, 10), new Vector2(150, 50)),
            string.Format("在线人数:{0}", onlineNum));
    }
    /// <summary>  
    /// 更新Serve端在线人数  
    /// </summary>  
    private void Update()
    {
        if (isServer)
            onlineNum = NetworkManager.singleton.numPlayers;
    }
    /// <summary>  
    /// 发送按钮响应事件  
    /// 将用户输入消息发送给服务端  
    /// </summary>  
    void SendButtonCallback()
    {
        if (!isLocalPlayer)
            return;
        if (inputBox.text.Length > 0)
        {
            string str = string.Format("{0}:{1}{2}", Network.player.ipAddress, System.Environment.NewLine, inputBox.text);
            CmdSend(str);
            inputBox.text = string.Empty;
        }
    }
    /// <summary>  
    /// 使用Command修饰的函数表示在客户端调用，在服务端执行  
    /// </summary>  
    /// <param name="str"></param>  
    [Command]
    void CmdSend(string str)
    {
        RpcShowMessage(str);
    }
    /// <summary>  
    /// ClientRpc修饰的函数 表示由服务端调用，在所有客户端执行  
    /// </summary>  
    /// <param name="str"></param>  
    [ClientRpc]
    void RpcShowMessage(string str)
    {
        GameObject item = Instantiate(itemPrefab, content);
        item.GetComponentInChildren<Text>().text = str;
    }
}