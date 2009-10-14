using System;
using NUnit.Framework;

using TagLib;
using TagLib.Exif;
using TagLib.IFD;
using TagLib.IFD.Entries;

namespace TagLib.Tests.FileFormats
{
    public static class AddImageMetadataTests
    {
		public static string test_comment = "&Test?Comment%$@_ ";

		public static File CreateTmpFile (string sample_file, string tmp_file)
		{
			if (System.IO.File.Exists (tmp_file))
                System.IO.File.Delete (tmp_file);

            System.IO.File.Copy (sample_file, tmp_file);

            return File.Create (tmp_file);
		}

		public static void AddExif (string sample_file, string tmp_file)
		{
			File file = CreateTmpFile (sample_file, tmp_file);

			ExifTag exif_tag = file.GetTag (TagTypes.Exif, false) as ExifTag;
			Assert.IsTrue (exif_tag == null);

			exif_tag = file.GetTag (TagTypes.Exif, true) as ExifTag;
			Assert.IsFalse (exif_tag == null);

			exif_tag.Comment = test_comment;

			Assert.AreEqual (test_comment, exif_tag.Comment);

			// Store and reload file
			file.Save ();
			file = File.Create (tmp_file);

			exif_tag = file.GetTag (TagTypes.Exif, false) as ExifTag;
			Assert.IsFalse (exif_tag == null);

			Assert.AreEqual (test_comment, exif_tag.Comment);

		}
	}
}