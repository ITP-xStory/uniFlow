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
    }*/
    [Serializable]
    public class InputAudioConfig
    {
        public String audioEncoding;
        public int sampleReateHertz;
        public String languageCode;
        public String[] phraseHints;
    }
    /*AudioEncoding
     * Audio encoding of the audio content sent in the conversational query request. 
     * Refer to the Cloud Speech API documentation for more details.
     */

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
}
