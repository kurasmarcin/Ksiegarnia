﻿using System.ComponentModel.DataAnnotations;

namespace Ksiegarnia.Models
{
    public class EmailData
    {
        public string From { get; set; }
       
        public string? To { get; set; }
       
        public string? Subject { get; set; }
        
        public string? Body { get; set; }

        public string Password { get; set; }

        public EmailData()
        {           
            From = "ksiegarnia.im@gmail.com";
            Password = "tkme gdux texk gyoz";                                  
        }
    }
}
