using GeoContacts.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoContacts.Services
{
    static class EmotionService
    {
        const string faceEndpoint = "https://westcentralus.api.cognitive.microsoft.com";
        readonly static Lazy<IFaceClient> faceApiClientHolder =
            new Lazy<IFaceClient>(() => new FaceClient(new ApiKeyServiceClientCredentials(CommonConstants.FaceApiKey)) { Endpoint = faceEndpoint });
        static IFaceClient FaceApiClient => faceApiClientHolder.Value;

        public static async Task<string> GetEmotionAsync(Stream stream)
        {
            var attributes = new List<FaceAttributeType> { { FaceAttributeType.Emotion } };
            var faceApiResponseList = await FaceApiClient.Face.DetectWithStreamAsync(stream, returnFaceAttributes: attributes);
            var emotion = faceApiResponseList.FirstOrDefault()?.FaceAttributes?.Emotion;


            if (emotion == null)
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
