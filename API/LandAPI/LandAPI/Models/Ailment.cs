﻿using System.Text.Json.Serialization;

namespace LandAPI.Models
{
	public class Ailment
	{
		[JsonPropertyName("id")]
		public int ID { get; set; }
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}
}