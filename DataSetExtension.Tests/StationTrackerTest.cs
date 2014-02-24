using System;
using NUnit.Framework;

namespace DataSetExtension.Tests
{
    [TestFixture]
    public class StationTrackerTest
    {
        [Test]
        public void Validate()
        {
            var tracker = new StationTracker();
            
            Assert.That(tracker.Validate("888", DateTime.Today), Is.True);
            
            tracker.Update("888", DateTime.Today);
            
            Assert.That(tracker.Validate("888", DateTime.Today.AddDays(1)), Is.True);       
            
            Assert.That(tracker.Validate("888", DateTime.Today), Is.False);
        }
        
        [Test]
        public void Update()
        {
            var tracker = new StationTracker();

            Assert.That(tracker.Update("123456", DateTime.Today), Is.True);
            
            Assert.That(tracker.Update("888", DateTime.Today), Is.True);
            
            Assert.That(tracker.Update("888", DateTime.Today.AddDays(1)), Is.False);
            
            Assert.That(tracker.Update("777", DateTime.Today), Is.True);
        }
    }
}