using Backend.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.DTOs.Response
{
    
    public class CommentResponseDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

       
    }
}
