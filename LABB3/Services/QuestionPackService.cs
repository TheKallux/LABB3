using LABB3.Models;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LABB3.Services;

internal class QuestionPackService
{
    private readonly string _folderPath;
    private readonly string _filePath;

    public QuestionPackService()
    {
        _folderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LABB3");
        _filePath = Path.Combine(_folderPath, "questionpacks.json");
    }

    public async Task SavePacksAsync(List<QuestionPack> packs)
    {
        try
        {
            if (!Directory.Exists(_folderPath))
                Directory.CreateDirectory(_folderPath);

            var json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving packs: {ex.Message}");
        }
    }

    public async Task<List<QuestionPack>> LoadPacksAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new List<QuestionPack>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<QuestionPack>>(json) ?? new List<QuestionPack>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading packs: {ex.Message}");
            return new List<QuestionPack>();
        }
    }
}