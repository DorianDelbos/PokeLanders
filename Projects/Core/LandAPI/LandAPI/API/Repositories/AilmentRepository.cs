﻿using System.Text.Json;

namespace LandAPI.API
{
    public class AilmentRepository
    {
        private readonly string _filePath = "wwwroot/Assets/Data/ailments.json";
        private List<Ailment> _ailment;

        public List<Ailment> Ailment => _ailment;

        public AilmentRepository()
        {
            if (File.Exists(_filePath))
            {
                string jsonData = File.ReadAllText(_filePath);
                _ailment = JsonSerializer.Deserialize<List<Ailment>>(jsonData)!;
            }

            _ailment ??= new List<Ailment>();
        }
    }
}
