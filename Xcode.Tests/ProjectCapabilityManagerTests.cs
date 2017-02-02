using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEditor.iOS.Xcode;

namespace Unity.PureCSharpTests.iOSExtensions
{
    [TestFixture]
    public class ProjectCapabilityManagerTests : GenericTester
    {
        public ProjectCapabilityManagerTests() : base("ProjectCapabilityManagerTestFiles", "ProjectCapabilityManagerTestOutput", false /*true for debug*/)
        {
        }
        
        private static void ResetGuidGenerator()
        {
            UnityEditor.iOS.Xcode.PBX.PBXGUID.SetGuidGenerator(LinearGuidGenerator.Generate);
            LinearGuidGenerator.Reset();
        }
        
        private void TestOutputProject(PBXProject proj, string testFilename)
        {
            string sourceFile = Path.Combine(GetTestSourcePath(), testFilename);
            string outputFile = Path.Combine(GetTestOutputPath(), testFilename);

            proj.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile),
                          "Output not equivalent to the expected output");

            PBXProject other = new PBXProject();
            other.ReadFromFile(outputFile);
            other.WriteToFile(outputFile);
            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile));
            if (!DebugEnabled())
                File.Delete(outputFile);
        }

        private void TestOutput(string output, string testFilename)
        {
            string sourceFile = Path.Combine(GetTestSourcePath(), testFilename);
            string outputFile = Path.Combine(GetTestOutputPath(), output);

            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile),
                          "Output not equivalent to the expected output");

            Assert.IsTrue(TestUtils.FileContentsEqual(outputFile, sourceFile));
            if (!DebugEnabled())
                File.Delete(outputFile);
        }
        
        private PBXProject ReadPBXProject()
        {
            return ReadPBXProject("base.pbxproj");
        }

        private PBXProject ReadPBXProject(string project)
        {
            PBXProject proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(Path.Combine(GetTestSourcePath(), project)));
            return proj;
        }

        private void SetupTestProject()
        {
            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());
            if (Directory.Exists(testProjectDir))
                Directory.Delete(testProjectDir, true);

            Directory.CreateDirectory(testProjectDir);

            if (File.Exists(Path.Combine(GetTestOutputPath(), "Info.plist")))
                File.Delete(Path.Combine(GetTestOutputPath(), "Info.plist"));

            File.Copy(Path.Combine(GetTestSourcePath(), "base_info.plist"), Path.Combine(GetTestOutputPath(), "Info.plist"));
            File.Copy(Path.Combine(GetTestSourcePath(), "base.entitlements"), Path.Combine(testProjectDir, "test.entitlements"));

            string xcodeProj = Path.Combine(GetTestOutputPath(), "Unity-iPhone.xcodeproj");
            if (Directory.Exists(xcodeProj))
                Directory.Delete(xcodeProj, true);

            Directory.CreateDirectory(xcodeProj);
            File.Copy(Path.Combine(GetTestSourcePath(), "base.pbxproj"), Path.Combine(xcodeProj, "project.pbxproj"));
        }

        [Test]
        public void AddiCloudWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddiCloud();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_icloud.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_icloud.entitlements");
        }

        [Test]
        public void AddPushNotificationsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddPushNotifications();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_pushnotification.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_pushnotification.entitlements");
        }

        [Test]
        public void AddGameCenterWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddGameCenter();
            capManager.Close();

            TestOutputProject(capManager.PBXProject, "add_gamecenter.pbxproj");
            TestOutput("Info.plist", "add_gamecenter.plist");
        }

        [Test]
        public void AddWalletWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddWallet(new string[] {"test1", "test2"});
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_wallet.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_wallet.entitlements");
        }

        [Test]
        public void AddSiriWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddSiri();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_siri.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_siri.entitlements");
        }

        [Test]
        public void AddApplePayWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddApplePay(new string[] {"test1", "test2"});
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_applepay.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_applepay.entitlements");
        }

        [Test]
        public void AddInAppPurchaseWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddInAppPurchase();
            capManager.Close();

            TestOutputProject(capManager.PBXProject, "add_iap.pbxproj");
        }

        [Test]
        public void AddMapsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddMaps(MapsOptions.Airplane);
            capManager.Close();

            TestOutputProject(capManager.PBXProject, "add_maps.pbxproj");
            TestOutput("Info.plist", "add_maps.plist");
        }

        [Test]
        public void AddPersonalVPNWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddPersonalVPN();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_personalvpn.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_personalvpn.entitlements");
        }

        [Test]
        public void AddBackgroundModesWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddBackgroundModes(BackgroundModesOptions.BackgroundFetch);
            capManager.Close();

            TestOutputProject(capManager.PBXProject, "add_backgroundmodes.pbxproj");
            TestOutput("Info.plist", "add_backgroundmodes.plist");
        }

        [Test]
        public void AddKeychainSharingWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddKeychainSharing(new string[] {"test1", "test2"});
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_keychainsharing.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_keychainsharing.entitlements");
        }

        [Test]
        public void AddInterAppAudioWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddInterAppAudio();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_interappaudio.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_interappaudio.entitlements");
        }

        [Test]
        public void AddAssociatedDomainsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddAssociatedDomains(new string[] {"webcredentials:example.com", "webcredentials:example2.com"});
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_associateddomains.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_associateddomains.entitlements");
        }

        [Test]
        public void AddAppGroupsWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddAppGroups(new string[] {"test1", "test2"});
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_appgroups.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_appgroups.entitlements");
        }

        [Test]
        public void AddHomeKitWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddHomeKit();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_homekit.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_homekit.entitlements");
        }

        [Test]
        public void AddDataProtectionWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddDataProtection();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_dataprotection.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_dataprotection.entitlements");
        }

        [Test]
        public void AddHealthKitWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddHealthKit();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_healthkit.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_healthkit.entitlements");
            TestOutput("Info.plist", "add_healthkit.plist");
        }

        [Test]
        public void AddWirelessAccessoryConfigurationWorks()
        {
            SetupTestProject();
            ResetGuidGenerator();

            var capManager = new ProjectCapabilityManager(GetTestOutputPath(), "test.entitlements");
            capManager.AddWirelessAccessoryConfiguration();
            capManager.Close();

            string testProjectDir = Path.Combine(GetTestOutputPath(), PBXProject.GetUnityTargetName());

            TestOutputProject(capManager.PBXProject, "add_wirelessaccessory.pbxproj");
            TestOutput(Path.Combine(testProjectDir, "test.entitlements"), "add_wirelessaccessory.entitlements");
        }
    }
}
