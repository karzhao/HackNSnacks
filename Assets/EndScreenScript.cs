using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;

public class EndScreenScript : MonoBehaviour
{
    public List<string> names;
    public TMP_Text myText; // start

    private string feedback; // want

    public GameObject summaryButton;

    private string apiKey = ""; // Replace with a secure method of storing keys
    private string apiUrl = "https://api.openai.com/v1/chat/completions";

    void Start()
    {
        string dataString = PlayerPrefs.GetString("MyData");
        names = new List<string>(dataString.Split(','));

        string foods = "You Ate: \n";
        foreach (string name in names)
        {
            foods += name + "\n";
        }
        myText.text = foods;

        getFeedback();
        // string prompt = $"In a day I ate these foods: {string.Join(", ", names)}. In less than 3 sentences, explain how balanced my diet was and how processed. Give feedback on macro/micro nutrients, variety, and healthiness. Be optimistic.";

        // StartCoroutine(GetAIResponse(prompt));
    }
    
    private void getFeedback() {
        int protein = PlayerPrefs.GetInt("TotalProtein", 0);
        int carbs = PlayerPrefs.GetInt("TotalCarbs", 0);
        int fat = PlayerPrefs.GetInt("TotalFat", 0);

        feedback =
        "Total nutrients eaten\n\n" +
        $"Protein: {protein} g\n" +
        $"Carbohydrates: {carbs} g\n" +
        $"Fat: {fat} g";
    }
    private IEnumerator GetAIResponse(string prompt)
    {
        // Manually construct the JSON string
        string jsonRequest = "{"
        + "\"model\": \"gpt-3.5-turbo\","  
        + "\"messages\": ["
        + "{ \"role\": \"system\", \"content\": \"You are a concise nutrition expert giving diet feedback.\" },"
        + "{ \"role\": \"user\", \"content\": \"" + EscapeJsonString(prompt) + "\" }"
        + "],"
        + "\"max_tokens\": 150"
        + "}";

        byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonRequest);

        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                myText.text = "Error connecting to AI API.";
            }
            else
            {
                string responseText = request.downloadHandler.text;
                // Debug.Log("Response: " + responseText);

                // Manually parse the response
                // Debug.Log(responseText);
                 feedback = ExtractFeedback(responseText);
                Debug.Log(feedback);
                
            }
        }
    }

    // Escape special characters for JSON
    private string EscapeJsonString(string str)
    {
        return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
    }

    // Extract AI response manually (basic parsing)
    private string ExtractFeedback(string json)
    {
        Debug.Log("Raw API Response: " + json); // Log the full response for debugging

        // Locate the "content" field in the JSON string
        string searchStr = "content";
        int startIndex = json.IndexOf(searchStr);
        Debug.Log("Starting index: " + startIndex);
        
        // If "content" is not found, return an error message
        if (startIndex == -1) return "No feedback received.";

        // Move startIndex past the search string
        startIndex += searchStr.Length + 4;
        
        // Find the closing quote of the content value
        int endIndex = json.IndexOf("refusal", startIndex)-12;
        Debug.Log(endIndex);
        
        // If no closing quote is found, return an error message
        if (endIndex == -1) return "No feedback received.";

        // Extract the content and replace escaped characters
        string extractedContent = json.Substring(startIndex, endIndex - startIndex)
                                    .Replace("\\n", "\n")  // Convert escaped newlines
                                    .Replace("\\\"", "\""); // Convert escaped quotes

        return extractedContent;
    }


    void Update()
    {
        // Debug.Log("test");
        // foreach (string s in names)
        // {
        //     Debug.Log(s);
        // }
    }

    public void switchToSummary(){
        myText.text = feedback;
        if (summaryButton != null)
        {
            summaryButton.gameObject.SetActive(false);
        }
    }
}
