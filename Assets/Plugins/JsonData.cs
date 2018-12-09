using System;
namespace JsonData
{
     /*
      "queryParams": {
        object (QueryParameters)
    },
      "queryInput": {
        object (QueryInput)
      },
      "inputAudio": string
    }*/

    //https://dialogflow.com/docs/reference/api-v2/rest/v2/projects.agent.sessions/detectIntent#QueryParameters
    [Serializable]
    public class RequestBody
    {
        public QueryInput queryInput;
        
    }

    /*
     * Required request body
     * queryInput
     * Represents the query input. It can contain either:
     * 1.An audio config which instructs the speech recognizer how to process the speech audio.
     * 2.A conversational query in the form of text,.
     * 3.An event that specifies which intent to trigger.
     * JSON representation
        {
          // Union field input can be only one of the following:
          "audioConfig": {
            object(InputAudioConfig)
          },
          "text": {
            object(TextInput)
          },
          "event": {
            object(EventInput)
          }
          // End of list of possible types for union field input.
        }*/
    [Serializable]
    public class QueryInput{
        public TextInput text;
        //public InputAudioConfig audioConfig;
    }
    /*
     * InputAudioConfig
     * Instructs the speech recognizer how to process the audio content.
    {
      "audioEncoding": enum(AudioEncoding),
      "sampleRateHertz": number,
      "languageCode": string,
      "phraseHints": [
        string
      ]
    }
    * phraseHints - is optional
    */
    [Serializable]
    public class InputAudioConfig
    {
        public AudioEncoding audioEncoding;
        public int sampleReateHertz;
        public String languageCode;
        public String[] phraseHints;
    }
    /*AudioEncoding
     * Audio encoding of the audio content sent in the conversational query request. 
     * Refer to the Cloud Speech API documentation for more details.
     */
    [Serializable]
    public enum AudioEncoding{
        AUDIO_ENCODING_UNSPECIFIED,
        AUDIO_ENCODING_LINEAR_16,
        AUDIO_ENCODING_FLAC,
        AUDIO_ENCODING_MULAW,
        AUDIO_ENCODING_AMR,
        AUDIO_ENCODING_AMR_WB,
        AUDIO_ENCODING_OGG_OPUS,
        AUDIO_ENCODING_SPEEX_WITH_HEADER_BYTE
    }

    //https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/WebhookState
    [Serializable]
    public enum WebhookState{
        STATE_UNSPECIFIED,
        WEBHOOK_STATE_ENABLED,
        WEBHOOK_STATE_ENABLED_FOR_SLOT_FILLING
    }

    public struct Format{

    }
    /*
     * TextInput
     * Represents the natural language text to be processed.
     * {
        "text": string,
        "languageCode": string
        }*/
    [Serializable]
    public class TextInput
    {
        public String text;
        public String languageCode;
    }

    /*response body json doc
     * {
          "responseId": string,
          "queryResult": {
            object(QueryResult)
          },
          "webhookStatus": {
            object(Status)
          }
        }
     */

    [Serializable]
    public class ResponseBody
    {
        public string responseId;
        public QueryResult queryResult;
        public Status webhookStatus;
    }

    //https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/QueryResult
    /*{
          "queryText": string,
          "languageCode": string,
          "speechRecognitionConfidence": number,
          "action": string,
          "parameters": {
            object
          },
          "allRequiredParamsPresent": boolean,
          "fulfillmentText": string,
          "fulfillmentMessages": [
            {
              object(Message)
            }
          ],
          "webhookSource": string,
          "webhookPayload": {
            object
          },
          "outputContexts": [
            {
              object(Context)
            }
          ],
          "intent": {
            object(Intent)
          },
          "intentDetectionConfidence": number,
          "diagnosticInfo": {
            object
          }}*/
    [Serializable]
    public class QueryResult{
        public string queryText;
        public string languageCode;
        public int speechRecognitionConfidence;
        public string action;
        public Object parameters;
        public bool allRequiredParamsPresent;
        public string fulfillmentText;
        public Message[] fulfillmentMessages;
        public string webhookSource;
        public Object webhookPayload;
        public Context[] outputContexts;
        public Intent intent;
        public int intentDetectionConfidence;
        public Object diagnosticInfo;
    }

    //https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/Operation#Status.SCHEMA_REPRESENTATION
    /*{
      "code": number,
      "message": string,
      "details": [
        {
          "@type": string,
          field1: ...,
          ...
        }
      ]
    }*/
    [Serializable]
    public class Status
    {
        public int code;
        public string message;
        public Object[] details;
    }

    //https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/Intent#SCHEMA_REPRESENTATION
    [Serializable]
    public class Intent{
        public string name;
        public string displayName;
        public WebhookState webhookState;
        public int priority;
        public bool isFallback;
    }
    //https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/Context
    [Serializable]
    public class Context{
        public string name;
    }

    [Serializable]
    public class Message{
        public Text[] text;
    }

    [Serializable]
    public class Text{
        public string text;
    }
}
