using System;
using NUnit.Framework;
using TagLib;
using TagLib.IFD;
using TagLib.IFD.Entries;
using TagLib.Jpeg;
using TagLib.Xmp;

namespace TagLib.Tests.FileFormats
{
    [TestFixture]
    public class JpegPanasonicTest
    {
		private static string sample_file = "samples/sample_panasonic.jpg";
		private static string tmp_file = "samples/tmwrite_panasonic.jpg";

		private Image.File file;

		private TagTypes contained_types = TagTypes.TiffIFD | TagTypes.XMP;

        [TestFixtureSetUp]
        public void Init()
        {
            file = File.Create (sample_file) as Image.File;
        }

		[Test]
		public void TestJpegRead()
		{
			Assert.IsTrue (file is Jpeg.File);

			Assert.AreEqual (contained_types, file.TagTypes);
			Assert.AreEqual (contained_types, file.TagTypesOnDisk);
		}

		[Test]
		public void TestExif()
		{
			var tag = file.GetTag (TagTypes.TiffIFD) as IFDTag;
			Assert.IsFalse (tag == null);

			var exif_ifd = tag.Structure.GetEntry(0, IFDEntryTag.ExifIFD) as SubIFDEntry;
			Assert.IsFalse (exif_ifd == null);
			var exif_tag = exif_ifd.Structure;

			Assert.AreEqual ("Panasonic", tag.Make);
			Assert.AreEqual ("DMC-FX35", tag.Model);
			Assert.AreEqual (100, tag.ISOSpeedRatings);
			Assert.AreEqual (1.0d/80.0d, tag.ExposureTime);
			Assert.AreEqual (2.8d, tag.FNumber);
			Assert.AreEqual (4.4d, tag.FocalLength);
			Assert.AreEqual (new DateTime (2009, 06, 26, 12, 58, 30), tag.DateTime);
			Assert.AreEqual (new DateTime (2009, 06, 26, 14, 58, 30), tag.DateTimeDigitized);
			Assert.AreEqual (new DateTime (2009, 06, 26, 12, 58, 30), tag.DateTimeOriginal);
		}

		[Test]
		public void TestMakernoteRead()
		{
			IFDTag tag = file.GetTag (TagTypes.TiffIFD) as IFDTag;
			Assert.IsFalse (tag == null);

			var makernote_ifd =
				tag.ExifIFD.GetEntry (0, (ushort) ExifEntryTag.MakerNote) as SubIFDEntry;
			Assert.IsFalse (makernote_ifd == null);

			var structure = makernote_ifd.Structure;
			Assert.IsFalse (structure == null);

			{
				var entry = structure.GetEntry (0, 0x01) as ShortIFDEntry;
				Assert.IsFalse (entry == null);
				Assert.AreEqual (2, entry.Value);
			}
			{
				var entry = structure.GetEntry (0, 0x03) as ShortIFDEntry;
				Assert.IsFalse (entry == null);
				Assert.AreEqual (1, entry.Value);
			}
			{
				var entry = structure.GetEntry (0, 0x07) as ShortIFDEntry;
				Assert.IsFalse (entry == null);
				Assert.AreEqual (1, entry.Value);
			}
			{
				var entry = structure.GetEntry (0, 0x29) as LongIFDEntry;
				Assert.IsFalse (entry == null);
				Assert.AreEqual (2286, entry.Value);
			}
			{
				var entry = structure.GetEntry (0, 0x26) as UndefinedIFDEntry;
				ByteVector read_bytes = entry.Data;
				ByteVector expected_bytes = new ByteVector (new byte [] {48, 50, 54, 48});

				Assert.AreEqual (expected_bytes.Count, read_bytes.Count);
				for (int i = 0; i < expected_bytes.Count; i++)
					Assert.AreEqual (expected_bytes[i], read_bytes[i]);
			}
		}
	}
}