﻿namespace HexaEngine.DirectXTex.Tests
{
    public unsafe class ImageIO : IDisposable
    {
        private const string DDSFilename = "assets\\textures\\test.dds";
        private const string HDRFilename = "assets\\textures\\test.hdr";
        private const string TGAFilename = "assets\\textures\\test.tga";
        private const string WICFilename = "assets\\textures\\test.png";

        private static byte[] LoadTexture(string path) => File.ReadAllBytes(path);

        [Test]
        public void LoadAndSaveFromDDSMemory()
        {
            ScratchImage image = new();
            Span<byte> src = LoadTexture(DDSFilename);
            TexBlob blob = new();

            DirectXTex.LoadFromDDSMemory(src, DDSFlags.None, &image);
            DirectXTex.SaveToDDSMemory(&image, DDSFlags.None, &blob);

            Span<byte> dest = blob.ToBytes();
            Assert.True(src.SequenceEqual(dest));

            blob.Release();
            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromDDSFile()
        {
            var path = Path.Combine("results", nameof(LoadAndSaveFromDDSFile), "test.dds");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            ScratchImage image = new();

            DirectXTex.LoadFromDDSFile(DDSFilename, DDSFlags.None, &image);
            DirectXTex.SaveToDDSFile(&image, DDSFlags.None, path);

            Span<byte> src = LoadTexture(DDSFilename);
            Span<byte> dest = LoadTexture(path);
            Assert.True(src.SequenceEqual(dest));

            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromHDRMemory()
        {
            ScratchImage image = new();
            Span<byte> src = LoadTexture(HDRFilename);
            TexBlob blob = new();

            DirectXTex.LoadFromHDRMemory(src, &image);
            DirectXTex.SaveToHDRMemory(&image, 0, &blob);

            Span<byte> dest = blob.ToBytes();
            Assert.True(src.SequenceEqual(dest));

            blob.Release();
            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromHDRFile()
        {
            var path = Path.Combine("results", nameof(LoadAndSaveFromHDRFile), "test.hdr");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            ScratchImage image = new();

            DirectXTex.LoadFromHDRFile(HDRFilename, &image);
            DirectXTex.SaveToHDRFile(&image, 0, path);

            Span<byte> src = LoadTexture(HDRFilename);
            Span<byte> dest = LoadTexture(path);
            Assert.True(src.SequenceEqual(dest));

            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromTGAMemory()
        {
            ScratchImage image = new();
            Span<byte> src = LoadTexture(TGAFilename);
            TexBlob blob = new();

            DirectXTex.LoadFromTGAMemory(src, TGAFlags.None, &image);
            DirectXTex.SaveToTGAMemory(&image, 0, TGAFlags.None, &blob);

            Span<byte> dest = blob.ToBytes();
            Assert.True(src.SequenceEqual(dest));

            blob.Release();
            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromTGAFile()
        {
            var path = Path.Combine("results", nameof(LoadAndSaveFromTGAFile), "test.tga");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            ScratchImage image = new();

            DirectXTex.LoadFromTGAFile(TGAFilename, TGAFlags.None, &image);
            DirectXTex.SaveToTGAFile(&image, 0, 0, path);

            Span<byte> src = LoadTexture(TGAFilename);
            Span<byte> dest = LoadTexture(path);
            Assert.True(src.SequenceEqual(dest));

            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromWICMemory()
        {
            ScratchImage image = new();
            Span<byte> src = LoadTexture(WICFilename);
            TexBlob blob = new();

            DirectXTex.LoadFromWICMemory(src, WICFlags.None, &image, null);
            DirectXTex.SaveToWICMemory(&image, WICFlags.None, DirectXTex.GetWICCodec(WICCodecs.PNG), &blob);

            Span<byte> dest = blob.ToBytes();
            Assert.True(src.SequenceEqual(dest));

            blob.Release();
            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        [Test]
        public void LoadAndSaveFromWICFile()
        {
            var path = Path.Combine("results", nameof(LoadAndSaveFromTGAFile), "test.png");
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            ScratchImage image = new();

            DirectXTex.LoadFromWICFile(WICFilename, WICFlags.None, &image);
            DirectXTex.SaveToWICFile(&image, 0, DirectXTex.GetWICCodec(WICCodecs.PNG), path);

            Span<byte> src = LoadTexture(WICFilename);
            Span<byte> dest = LoadTexture(path);
            Assert.True(src.SequenceEqual(dest));

            image.Release();
            if (image.pScratchImage != null)
                Trace.Fail("Mem leak");
        }

        public void Dispose()
        {
            if (Directory.Exists("results"))
                Directory.Delete("results", true);
            GC.SuppressFinalize(this);
        }
    }
}