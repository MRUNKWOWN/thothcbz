namespace ThothCbz.Actions
{
    public class MeanRemovalFilter
    {
        // The 3x3 Mean Removal kernel
        private static readonly int[,] Kernel = new int[3, 3]
        {
        { -1, -1, -1 },
        { -1,  9, -1 },
        { -1, -1, -1 }
        };

        public static byte[,] Apply(byte[,] image)
        {
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            byte[,] result = new byte[width, height];

            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    int sum = 0;

                    for (int ky = 0; ky < 3; ky++)
                    {
                        for (int kx = 0; kx < 3; kx++)
                        {
                            int px = x + kx - 1;
                            int py = y + ky - 1;
                            sum += image[px, py] * Kernel[kx, ky];
                        }
                    }

                    // Clamp the value to 0-255
                    sum = Math.Min(255, Math.Max(0, sum));
                    result[x, y] = (byte)sum;
                }
            }

            // Optionally: copy border pixels from the original image
            for (int x = 0; x < width; x++)
            {
                result[x, 0] = image[x, 0];
                result[x, height - 1] = image[x, height - 1];
            }
            for (int y = 0; y < height; y++)
            {
                result[0, y] = image[0, y];
                result[width - 1, y] = image[width - 1, y];
            }

            return result;
        }
    }
}
