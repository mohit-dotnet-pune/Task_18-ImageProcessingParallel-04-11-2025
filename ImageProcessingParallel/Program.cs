using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing; // For Resize

namespace ParallelImageProcessing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFolder = @"C:\Users\Mohit.Bagul\source\repos\ImageProcessingParallel\ImageProcessingParallel\Input\";   // 📂 Folder with source images
            string outputFolder = @"C:\Users\Mohit.Bagul\source\repos\ImageProcessingParallel\ImageProcessingParallel\Output\"; // 📂 Folder to save thumbnails

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            // 1️⃣ Get image file paths
            string[] imageFiles = Directory.GetFiles(inputFolder, "*.*")
                                           .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                       f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                                           .ToArray();

            Console.WriteLine($"Found {imageFiles.Length} images. Processing in parallel...\n");

            // 2️⃣ Process images in parallel
            Parallel.ForEach(imageFiles, imagePath =>
            {
                try
                {
                    using (Image image = Image.Load(imagePath))
                    {
                        // 3️⃣ Resize image to thumbnail
                        image.Mutate(x => x.Resize(150, 150));

                        string fileName = Path.GetFileName(imagePath);
                        string outputPath = Path.Combine(outputFolder, fileName);

                        // 4️⃣ Save thumbnail
                        image.Save(outputPath);
                        Console.WriteLine($"✅ Processed: {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error processing {imagePath}: {ex.Message}");
                }
            });

            Console.WriteLine("\nAll images processed successfully!");
        }
    }
}
