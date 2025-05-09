﻿using System.Text.Json;

namespace LandAPI.API
{
    public class NatureRepository
    {
        private readonly string _filePath = "wwwroot/Assets/Data/nature.json";
        private List<Nature> _nature;

        public List<Nature> Nature => _nature;

        public NatureRepository()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                _nature = JsonSerializer.Deserialize<List<Nature>>(jsonData)!;
            }

            _nature ??= new List<Nature>();
        }
    }
}
