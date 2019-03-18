using System;
using System.Collections.Generic;
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
	//https://dialogflow.com/docs/reference/api-v2/rest/v2beta1/projects.agent.sessions/detectIntent
	//https://dialogflow.com/docs/reference/api-v2/rest/v2/projects.agent.sessions/detectIntent#QueryParameters
	[Serializable]
	public class RequestBody
	{
		public QueryInput queryInput;
		/*  
            string (bytes format)
            Optional. 
            The natural language speech audio to be processed. 
            This field should be populated if queryInput is set to an input audio config. 
            A single request can contain up to 1 minute of speech audio data.
            A base64-encoded string.
        */

		public string inputAudio;
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
	public class QueryInput
	{
		//public TextInput text;
		public InputAudioConfig audioConfig;
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
		public int sampleRateHertz;
		public String languageCode;
		public String[] phraseHints;
	}
	/*AudioEncoding
     * Audio encoding of the audio content sent in the conversational query request. 
     * Refer to the Cloud Speech API documentation for more details.
     */
	[Serializable]
	public enum AudioEncoding
	{
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
	public enum WebhookState
	{
		STATE_UNSPECIFIED,
		WEBHOOK_STATE_ENABLED,
		WEBHOOK_STATE_ENABLED_FOR_SLOT_FILLING
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
	public class QueryResult
	{
		public string queryText;
		public string languageCode;
		public int speechRecognitionConfidence;
		public string action;
		public Struct parameters;
		public bool allRequiredParamsPresent;
		public string fulfillmentText;
		public Message[] fulfillmentMessages;
		public string webhookSource;
		public Struct webhookPayload;
		public Context[] outputContexts;
		public Intent intent;
		public int intentDetectionConfidence;
		public Struct diagnosticInfo;
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
	public class Intent
	{
		public string name;
		public string displayName;
		public WebhookState webhookState;
		public int priority;
		public bool isFallback;
	}

	//https://dialogflow.com/docs/reference/api-v2/rest/Shared.Types/Context
	[Serializable]
	public class Context
	{
		public string name;
	}

	[Serializable]
	public class Struct
	{
		public Dictionary<string, Value> fields;
	}

	[Serializable]
	public class Value
	{
		public NullValue null_value;
		public double number_value;
		public string string_value;
		public bool bool_value;
		public Struct struct_value;
		public ListValue list_value;

		public void ForBool(bool value){
			this.bool_value = value;
		}

		public void ForString(string value)
		{
			this.string_value = value;
		}

		public void ForNumber(double number){
			this.number_value = number;
		}

		public void ForNull()
		{
			this.null_value = NullValue.null_vaule;
		}

		public void ForStruct(Struct value){
			this.struct_value = value;
		}

		public void ForList(ListValue value){
			this.list_value = value;
		}
	}

	[Serializable]
	public enum NullValue
	{
		null_vaule
	}

	[Serializable]
	public class ListValue{
		public Value values;

	}

	[Serializable]
	public class Text{
		public string[] text;
	}

	[Serializable]
	public class Message{
		public Text text;
	}




}