using GeoContacts.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeoContacts.Services
{
    static class EmotionService
    {   readonly static Lazy<FaceAPI> faceApiClientHolder =
            new Lazy<FaceAPI>(() => new FaceAPI(new ApiKeyServiceClientCredentials(CommonConstants.FaceApiKey)) { AzureRegion = AzureRegions.Westcentralus });
        static FaceAPI FaceApiClient => faceApiClientHolder.Value;

        public static async Task<string> GetEmotionAsync(Stream stream)
        {
            var attributes = new List<FaceAttributeType> { { FaceAttributeType.Emotion } };
            var faceApiResponseList = await FaceApiClient.Face.DetectWithStreamAsync(stream, returnFaceAttributes: attributes);
            var emotion = faceApiResponseList.FirstOrDefault()?.FaceAttributes?.Emotion;


            if(emotion == null)
                return "🐵";

            var scores = new Dictionary<string, double>
            {
                ["😠"] = emotion.Anger,
                ["🙄"] = emotion.Contempt,
                ["🤢"] = emotion.Disgust,
                ["😨"] = emotion.Fear,
                ["😃"] = emotion.Happiness,
                ["😐"] = emotion.Neutral,
                ["😢"] = emotion.Sadness,
                ["😲"] = emotion.Surprise,
            };

            return scores.OrderByDescending(x => x.Value).First().Key;
        }     
    }
}
