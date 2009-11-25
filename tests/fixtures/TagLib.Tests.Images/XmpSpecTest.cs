using System;
using NUnit.Framework;
using TagLib;
using TagLib.Xmp;

namespace TagLib.Tests.Images
{
	/// <summary>
	///    This validates some of the examples in the specification.
	/// </summary>
    [TestFixture]
    public class XmpSpecTest
    {
		[Test]
		public void SimpleTypeTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:xmp=""http://ns.adobe.com/xap/1.0/"">
							<xmp:CreateDate>2002-08-15T17:10:04Z</xmp:CreateDate>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);
			Assert.AreEqual (XmpNodeType.Simple, tree.Children[0].Type);
			Assert.AreEqual (XmpTag.XAP_NS, tree.Children[0].Namespace);
			Assert.AreEqual ("CreateDate", tree.Children[0].Name);
			Assert.AreEqual ("2002-08-15T17:10:04Z", tree.Children[0].Value);
		}

		[Test]
		public void SimpleTypeShorthandTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:xmp=""http://ns.adobe.com/xap/1.0/""
						   xmp:CreateDate=""2002-08-15T17:10:04Z""/>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);
			Assert.AreEqual (XmpNodeType.Simple, tree.Children[0].Type);
			Assert.AreEqual (XmpTag.XAP_NS, tree.Children[0].Namespace);
			Assert.AreEqual ("CreateDate", tree.Children[0].Name);
			Assert.AreEqual ("2002-08-15T17:10:04Z", tree.Children[0].Value);
		}

		[Test]
		public void StructuredTypeTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:xmpTPg=""http://ns.adobe.com/xap/1.0/t/pg/"">
							<xmpTPg:MaxPageSize>
								<rdf:Description xmlns:stDim=""http:ns.adobe.com/xap/1.0/sType/Dimensions#"">
									<stDim:w>4</stDim:w>
									<stDim:h>3</stDim:h>
									<stDim:unit>inch</stDim:unit>
								</rdf:Description>
							</xmpTPg:MaxPageSize>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			var PG_NS = "http://ns.adobe.com/xap/1.0/t/pg/";
			var DIM_NS = "http:ns.adobe.com/xap/1.0/sType/Dimensions#";

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);

			var node = tree.Children[0];
			Assert.AreEqual (XmpNodeType.Simple, node.Type);
			Assert.AreEqual (PG_NS, node.Namespace);
			Assert.AreEqual ("MaxPageSize", node.Name);
			Assert.AreEqual (3, node.Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[0].Namespace);
			Assert.AreEqual ("w", node.Children[0].Name);
			Assert.AreEqual ("4", node.Children[0].Value);
			Assert.AreEqual (0, node.Children[0].Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[1].Namespace);
			Assert.AreEqual ("h", node.Children[1].Name);
			Assert.AreEqual ("3", node.Children[1].Value);
			Assert.AreEqual (0, node.Children[1].Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[2].Namespace);
			Assert.AreEqual ("unit", node.Children[2].Name);
			Assert.AreEqual ("inch", node.Children[2].Value);
			Assert.AreEqual (0, node.Children[2].Children.Count);
		}

		[Test]
		public void StructuredTypeShorthandTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:xmpTPg=""http://ns.adobe.com/xap/1.0/t/pg/"">
							<xmpTPg:MaxPageSize rdf:parseType=""Resource"" xmlns:stDim=""http:ns.adobe.com/xap/1.0/sType/Dimensions#"">
								<stDim:w>4</stDim:w>
								<stDim:h>3</stDim:h>
								<stDim:unit>inch</stDim:unit>
							</xmpTPg:MaxPageSize>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			var PG_NS = "http://ns.adobe.com/xap/1.0/t/pg/";
			var DIM_NS = "http:ns.adobe.com/xap/1.0/sType/Dimensions#";

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);

			var node = tree.Children[0];
			Assert.AreEqual (XmpNodeType.Simple, node.Type);
			Assert.AreEqual (PG_NS, node.Namespace);
			Assert.AreEqual ("MaxPageSize", node.Name);
			Assert.AreEqual (3, node.Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[0].Namespace);
			Assert.AreEqual ("w", node.Children[0].Name);
			Assert.AreEqual ("4", node.Children[0].Value);
			Assert.AreEqual (0, node.Children[0].Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[1].Namespace);
			Assert.AreEqual ("h", node.Children[1].Name);
			Assert.AreEqual ("3", node.Children[1].Value);
			Assert.AreEqual (0, node.Children[1].Children.Count);

			Assert.AreEqual (DIM_NS, node.Children[2].Namespace);
			Assert.AreEqual ("unit", node.Children[2].Name);
			Assert.AreEqual ("inch", node.Children[2].Value);
			Assert.AreEqual (0, node.Children[2].Children.Count);
		}

		[Test]
		public void ArrayTypeTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:dc=""http://purl.org/dc/elements/1.1/"">
							<dc:subject>
								<rdf:Bag>
									<rdf:li>metadata</rdf:li>
									<rdf:li>schema</rdf:li>
									<rdf:li>XMP</rdf:li>
								</rdf:Bag>
							</dc:subject>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);

			var node = tree.Children[0];
			Assert.AreEqual (XmpNodeType.Bag, node.Type);
			Assert.AreEqual (XmpTag.DC_NS, node.Namespace);
			Assert.AreEqual ("subject", node.Name);
			Assert.AreEqual (3, node.Children.Count);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[0].Namespace);
			Assert.AreEqual ("li", node.Children[0].Name);
			Assert.AreEqual ("metadata", node.Children[0].Value);
			Assert.AreEqual (0, node.Children[0].Children.Count);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[1].Namespace);
			Assert.AreEqual ("li", node.Children[1].Name);
			Assert.AreEqual ("schema", node.Children[1].Value);
			Assert.AreEqual (0, node.Children[1].Children.Count);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[2].Namespace);
			Assert.AreEqual ("li", node.Children[2].Name);
			Assert.AreEqual ("XMP", node.Children[2].Value);
			Assert.AreEqual (0, node.Children[2].Children.Count);
		}

		[Test]
		public void QualifierTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description
								rdf:about=""""
								xmlns:dc=""http://purl.org/dc/elements/1.1/""
								xmlns:ns=""ns:myNamespace/"">
							<dc:creator>
								<rdf:Seq>
									<rdf:li>
										<rdf:Description>
											<rdf:value>William Gilbert</rdf:value>
											<ns:role>lyricist</ns:role>
										</rdf:Description>
									</rdf:li>
									<rdf:li>
										<rdf:Description >
											<rdf:value>Arthur Sullivan</rdf:value>
											<ns:role>composer</ns:role>
										</rdf:Description>
									</rdf:li>
								</rdf:Seq>
							</dc:creator>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);

			var node = tree.Children[0];
			Assert.AreEqual (XmpNodeType.Seq, node.Type);
			Assert.AreEqual (XmpTag.DC_NS, node.Namespace);
			Assert.AreEqual ("creator", node.Name);
			Assert.AreEqual (2, node.Children.Count);

			// TODO: The stuff below will fail, this is a known bug, it needs fixing.
			// Check the spec on page 20 for the parsing diagram.

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[0].Namespace);
			Assert.AreEqual ("li", node.Children[0].Name);
			Assert.AreEqual ("William Gilbert", node.Children[0].Value);
			Assert.AreEqual (0, node.Children[0].Children.Count);
			Assert.AreEqual ("lyricist", node.Children[0].GetQualifier ("ns:myNamespace/", "role").Value);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[1].Namespace);
			Assert.AreEqual ("li", node.Children[1].Name);
			Assert.AreEqual ("Arthur Sullivan", node.Children[1].Value);
			Assert.AreEqual (0, node.Children[1].Children.Count);
			Assert.AreEqual ("composer", node.Children[1].GetQualifier ("ns:myNamespace/", "role").Value);
		}

		[Test]
		public void LangAltTest ()
		{
			string metadata =
				@"<x:xmpmeta xmlns:x=""adobe:ns:meta/"">
					<rdf:RDF xmlns:rdf=""http://www.w3.org/1999/02/22-rdf-syntax-ns#"">
						<rdf:Description rdf:about="""" xmlns:xmp=""http://ns.adobe.com/xap/1.0/"">
							<xmp:Title>
								<rdf:Alt>
									<rdf:li xml:lang=""x-default"">XMP - Extensible Metadata Platform</rdf:li>
									<rdf:li xml:lang=""en-us"">XMP - Extensible Metadata Platform</rdf:li>
									<rdf:li xml:lang=""fr-fr"">XMP - Une Platforme Extensible pour les Métadonnées</rdf:li>
									<rdf:li xml:lang=""it-it"">XMP - Piattaforma Estendibile di Metadata</rdf:li>
								</rdf:Alt>
							</xmp:Title>
						</rdf:Description>
					</rdf:RDF>
				</x:xmpmeta>";

			XmpTag tag = new XmpTag (metadata);
			var tree = tag.NodeTree;

			Assert.IsTrue (tree != null);
			Assert.AreEqual (1, tree.Children.Count);

			var node = tree.Children[0];
			Assert.AreEqual (XmpNodeType.Alt, node.Type);
			Assert.AreEqual (XmpTag.XAP_NS, node.Namespace);
			Assert.AreEqual ("Title", node.Name);
			Assert.AreEqual (4, node.Children.Count);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[0].Namespace);
			Assert.AreEqual ("li", node.Children[0].Name);
			Assert.AreEqual ("XMP - Extensible Metadata Platform", node.Children[0].Value);
			Assert.AreEqual (0, node.Children[0].Children.Count);
			Assert.AreEqual ("x-default", node.Children[0].GetQualifier (XmpTag.XML_NS, "lang").Value);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[1].Namespace);
			Assert.AreEqual ("li", node.Children[1].Name);
			Assert.AreEqual ("XMP - Extensible Metadata Platform", node.Children[1].Value);
			Assert.AreEqual (0, node.Children[1].Children.Count);
			Assert.AreEqual ("en-us", node.Children[1].GetQualifier (XmpTag.XML_NS, "lang").Value);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[2].Namespace);
			Assert.AreEqual ("li", node.Children[2].Name);
			Assert.AreEqual ("XMP - Une Platforme Extensible pour les Métadonnées", node.Children[2].Value);
			Assert.AreEqual (0, node.Children[2].Children.Count);
			Assert.AreEqual ("fr-fr", node.Children[2].GetQualifier (XmpTag.XML_NS, "lang").Value);

			Assert.AreEqual (XmpTag.RDF_NS, node.Children[3].Namespace);
			Assert.AreEqual ("li", node.Children[3].Name);
			Assert.AreEqual ("XMP - Piattaforma Estendibile di Metadata", node.Children[3].Value);
			Assert.AreEqual (0, node.Children[3].Children.Count);
			Assert.AreEqual ("it-it", node.Children[3].GetQualifier (XmpTag.XML_NS, "lang").Value);
		}
	}
}