using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaSharp.Sample.Mvc.ModelsJaime
{
    
    public class MediaJaime
    {
        public InstaSharp.Models.Media Media { get; set; }

        public int TotalLikes { get; set; } // just for testing. we can use InstaSharp.Models.Media.Likes

        public List<string> UsuariosQueDeramLike { get; set; }

        public Dictionary<DateTime, int> QtdComentariosPorDia { get; set; }
    }
}