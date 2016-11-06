﻿using Microsoft.Build.Framework;

namespace DataTransferObjects
{
    public class UpdateEmployeeTitleDTO
    {
        /// <summary>
        /// The employee title.
        /// </summary>
        [Required]
        public string Title { get; set; }

    }
}