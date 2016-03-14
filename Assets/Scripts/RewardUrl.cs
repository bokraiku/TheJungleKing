using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Security.Cryptography;
using System.Text;
using SimpleJson;
using System.Collections;


public class RewardUrl : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick(PointerEventData eventData) {
		string name = PlayerPrefs.GetString ("name");
        JsonObject top = RootScope.instance.Query<JsonObject>("top");

        
        // Create Timestamp
        TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
        double timestamp = (double)span.TotalSeconds;

        //check sum
        string str_checksum = "TarzanGame|" + name + "|" + timestamp;
        
        //Crreate MD5
        MD5 md5Hash = MD5.Create();
        string checksum = GetMd5Hash(md5Hash, str_checksum);
        
        string URL = "http://tarzan.hirichclub.com/member/register.php?uid=" + name + "&tm=" + timestamp.ToString() + "&chksum=" + checksum+"&round="+ top["round_id"].ToString();
        Debug.Log("Request URL : " + URL);
        Debug.Log("Round : " + top["round_id"].ToString());
        Application.OpenURL(URL);
	}


    public static string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    // Verify a hash against a string.
    static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
    {
        // Hash the input.
        string hashOfInput = GetMd5Hash(md5Hash, input);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
