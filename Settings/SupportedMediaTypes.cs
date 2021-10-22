using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Settings
{
    public class SupportedMediaTypes
    {
        public MediaType Image { get; set; }
        public MediaType Video { get; set; }
        public MediaType Audio { get; set; }
        public IDictionary<string, int> ContentTypesMaxSize()
        {
            var dictionary = new Dictionary<string, int>();
            var allMediaTypes = new List<MediaType> { Image, Video, Audio };

            foreach (var mediaType in allMediaTypes)
            {
                var maxFileSizeBytes = mediaType.MaxFileSizeMb * 1024 * 1024;
                foreach (var contentType in mediaType.ContentTypes)
                {
                    dictionary[contentType] = maxFileSizeBytes;
                }
            }

            return dictionary;
        }
    }

    public class MediaType
    {
        public IEnumerable<string> ContentTypes { get; set; }
        public int MaxFileSizeMb { get; set; }
    }
}

