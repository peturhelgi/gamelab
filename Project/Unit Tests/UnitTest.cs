
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using TheGreatEscape.GameLogic.Collision;

namespace Unit_Tests
{
    [TestClass]
    public class Collision
    {
        [TestMethod]
        public void AxisAllignesBoundingBoxes()
        {
            AxisAllignedBoundingBox a = new AxisAllignedBoundingBox(new Vector2(0, 0), new Vector2(2, 2));
            AxisAllignedBoundingBox b = new AxisAllignedBoundingBox(new Vector2(1, 1), new Vector2(3, 3));
            AxisAllignedBoundingBox c = new AxisAllignedBoundingBox(new Vector2(3, 3), new Vector2(4, 4));

            AxisAllignedBoundingBox d = new AxisAllignedBoundingBox(new Vector2(0.5f, 0.5f), new Vector2(1.5f, 1.5f));


            Assert.AreEqual(true, a.Intersects(b));
            Assert.AreEqual(true, b.Intersects(a));
            Assert.AreEqual(false, a.Intersects(c));
            Assert.AreEqual(false, c.Intersects(a));

            Assert.AreEqual(true, a.Intersects(d));
            Assert.AreEqual(true, d.Intersects(a));


        }
    }
}
