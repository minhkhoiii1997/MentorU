﻿using System.IO;
using System.Threading.Tasks;

namespace MentorU.Services
{

    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
    
}
