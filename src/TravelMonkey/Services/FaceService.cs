using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using TravelMonkey.Models;
using Xamarin.Forms;
using ApiKeyServiceClientCredentials = Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials;

namespace TravelMonkey.Services
{

    public class FaceService
    {
        private readonly FaceClient _faceClient;
        
        public FaceService()
        {
            _faceClient = new FaceClient(new ApiKeyServiceClientCredentials(ApiKeys.FaceDetectionKey))
            {
                Endpoint = ApiKeys.FaceDetectionEndpoint
            };
            
            MessagingCenter.Subscribe<ComputerVisionService, Stream>(this, Constants.FaceDetected,async (sender, arg) => await GetFaceResult(arg) );
        }

        public async Task<FaceResult> GetFaceResult(Stream imageStream)
        {
            try
            {
                var result =
                    await _faceClient.Face.DetectWithStreamAsync(imageStream, true, true,
                        new List<FaceAttributeType> {FaceAttributeType.Emotion, FaceAttributeType.Gender, FaceAttributeType.Age });

                
                var faceResult = new FaceResult();
                foreach (var face in result)
                {
                    var emotionType = string.Empty;
                    var emotionValue = 0.0;
                    var emotion = face.FaceAttributes.Emotion;
                    if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
                    if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
                    if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
                    if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
                    if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
                    if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
                    if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
                    if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }

                    var faceDetails = new FaceDetails
                    {
                        Gender = face.FaceAttributes.Gender.ToString(),
                        Emotion = emotionType
                    };
                    faceResult.Details.Add(faceDetails);
                }

                return faceResult;
               
            }
            catch (Exception ex)
            {
                return new FaceResult();
            }
        }
        
    }
}