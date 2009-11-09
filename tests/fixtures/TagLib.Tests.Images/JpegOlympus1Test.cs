using System;
using NUnit.Framework;
using TagLib;
using TagLib.IFD;
using TagLib.IFD.Entries;
using TagLib.Jpeg;
using TagLib.Xmp;

namespace TagLib.Tests.Images
{
	[TestFixture]
	public class JpegOlympus1Test
	{
		private static string sample_file = "samples/sample_olympus1.jpg";
		private static string tmp_file = "samples/tmpwrite_olympus1.jpg";

		private TagTypes contained_types = TagTypes.TiffIFD | TagTypes.XMP;

		private File file;

		[TestFixtureSetUp]
		public void Init () {
			file = File.Create (sample_file);
		}

		[Test]
		public void JpegRead () {
			CheckTags (file);
		}

		[Test]
		public void ExifRead () {
			CheckExif (file);
		}

		[Test]
		public void MakernoteRead () {
			CheckMakerNote (file);
		}

		[Test]
		public void Rewrite () {
			File tmp = Utils.CreateTmpFile (sample_file, tmp_file);
			tmp.Save ();

			tmp = File.Create (tmp_file);

			// did not run at the moment, since XMP writing is not implemented
			//CheckTags (tmp);
			CheckExif (tmp);
			CheckMakerNote (tmp);
		}

		[Test]
		public void AddExif ()
		{
			AddImageMetadataTests.AddExifTest (sample_file, tmp_file, true);
		}

		[Test]
		public void AddGPS ()
		{
			AddImageMetadataTests.AddGPSTest (sample_file, tmp_file, true);
		}

		public void CheckTags (File file) {
			Assert.IsTrue (file is Jpeg.File, "not a Jpeg file");

			Assert.AreEqual (contained_types, file.TagTypes);
			Assert.AreEqual (contained_types, file.TagTypesOnDisk);
		}

		public void CheckExif (File file) {
			var tag = file.GetTag (TagTypes.TiffIFD) as IFDTag;

			Assert.IsNotNull (tag, "tag");

			var exif_ifd = tag.Structure.GetEntry(0, IFDEntryTag.ExifIFD) as SubIFDEntry;
			Assert.IsNotNull (exif_ifd, "Exif IFD");

			Assert.AreEqual ("OLYMPUS IMAGING CORP.  ", tag.Make);
			Assert.AreEqual ("u700,S700       ", tag.Model);
			Assert.AreEqual (64, tag.ISOSpeedRatings, "ISOSpeedRatings");
			Assert.AreEqual (1.0d/25.0d, tag.ExposureTime);
			Assert.AreEqual (3.4d, tag.FNumber);
			Assert.AreEqual (6.5d, tag.FocalLength);
			Assert.AreEqual (new DateTime (2006, 10, 23, 06, 57, 40), tag.DateTime);
			Assert.AreEqual (new DateTime (2006, 10, 23, 08, 57, 40), tag.DateTimeDigitized);
			Assert.AreEqual (new DateTime (2006, 10, 23, 06, 57, 40), tag.DateTimeOriginal);
		}


		public void CheckMakerNote (File file) {
			IFDTag tag = file.GetTag (TagTypes.TiffIFD) as IFDTag;
			Assert.IsNotNull (tag, "tag");

			var makernote_ifd =
				tag.ExifIFD.GetEntry (0, (ushort) ExifEntryTag.MakerNote) as SubIFDEntry;

			Assert.IsNotNull (makernote_ifd, "makernote ifd");
			Assert.AreEqual (SubIFDType.OlympusMakernote1, makernote_ifd.SubIFDType);

			var structure = makernote_ifd.Structure;
			Assert.IsNotNull (structure, "structure");
			{
				var entry = structure.GetEntry (0, 0x0200) as LongArrayIFDEntry;
				Assert.IsNotNull (entry, "entry 0x0200");
				uint[] values = entry.Values;

				Assert.IsNotNull (values, "values of entry 0x0200");
				Assert.AreEqual (3, values.Length);
				Assert.AreEqual (0, values[0]);
				Assert.AreEqual (0, values[1]);
				Assert.AreEqual (0, values[2]);
			}
			{
				var entry = structure.GetEntry (0, 0x0204) as RationalIFDEntry;
				Assert.IsNotNull (entry, "entry 0x0204");
				Assert.AreEqual (100.0d/100.0d, (double) entry.Value);
			}
			{
				var entry = structure.GetEntry (0, 0x0207) as StringIFDEntry;
				Assert.IsNotNull (entry, "entry 0x0207");
				Assert.AreEqual ("D4303", entry.Value);
			}
		}
	}
}