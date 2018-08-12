using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackageInstaller.Enums;
using PackageInstaller.Objects;
using PackageInstaller.Services;

namespace PackageInstallerTests.Services
{
    [TestClass]
    public class PackageInstallServiceTests
    {
        [TestMethod]
        public void TestInstallPackagesValid_Easy()
        {
            List<string> input = new List<string>()
            {
                "KittenService: CamelCaser",
                "CamelCaser: "
            };

            PackageInstallResponse response = PackageInstallService.InstallPackages(input);

            // The response should never be null
            Assert.AreNotEqual(response, null);

            // The response should be successful with this result
            Assert.AreEqual(response.Status, PackageInstallStatuses.SUCCESS);
            Assert.AreEqual(response.InstalledPackages, "CamelCaser, KittenService");
        }

        [TestMethod]
        public void TestInstallPackagesValid_Medium()
        {
            List<string> input = new List<string>()
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: Leetmeme",
                "Ice: "
            };

            PackageInstallResponse response = PackageInstallService.InstallPackages(input);

            // The response should never be null
            Assert.AreNotEqual(response, null);

            // The response should be successful with this result
            Assert.AreEqual(response.Status, PackageInstallStatuses.SUCCESS);
            Assert.AreEqual(response.InstalledPackages, "KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream");
        }

        [TestMethod]
        public void TestInstallPackagesInvalid_Medium()
        {
            List<string> input = new List<string>()
            {
                "KittenService: ",
                "Leetmeme: Cyberportal",
                "Cyberportal: Ice",
                "CamelCaser: KittenService",
                "Fraudstream: ",
                "Ice: Leetmeme"
            };

            PackageInstallResponse response = PackageInstallService.InstallPackages(input);

            // The response should never be null
            Assert.AreNotEqual(response, null);

            // The response should be successful with this result
            Assert.AreEqual(response.Status, PackageInstallStatuses.CONTAINS_CYCLE);
        }
    }
}
