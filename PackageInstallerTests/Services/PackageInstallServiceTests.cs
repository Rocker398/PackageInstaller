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
            string[] input = new string[] 
            {
                "KittenService: CamelCaser",
                "CamelCaser: "
            };

            PackageInstallResponse response = PackageInstallService.InstallPackages(input);

            // The response should never be null
            Assert.AreNotEqual(null, response);

            // The response should be successful with this result
            Assert.AreEqual(PackageInstallStatuses.SUCCESS, response.Status);
            Assert.AreEqual("CamelCaser, KittenService", response.InstalledPackages);
        }

        [TestMethod]
        public void TestInstallPackagesValid_Medium()
        {
            string[] input = new string[]
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
            Assert.AreNotEqual(null, response);

            // The response should be successful with this result
            Assert.AreEqual(PackageInstallStatuses.SUCCESS, response.Status);
            Assert.AreEqual("KittenService, Ice, CamelCaser, Cyberportal, Leetmeme, Fraudstream", response.InstalledPackages);
        }

        [TestMethod]
        public void TestInstallPackagesValid_Hard()
        {
            string[] input = new string[]
            {
                "Texture: Cabbage",
                "Bread: Amusement",
                "Cabbage: Giraffe",
                "Amusement: Giraffe",
                "Giraffe: Flower",
                "Flower: "
            };

            PackageInstallResponse response = PackageInstallService.InstallPackages(input);

            // The response should never be null
            Assert.AreNotEqual(null, response);

            // The response should be successful with this result
            Assert.AreEqual(PackageInstallStatuses.SUCCESS, response.Status);
            Assert.AreEqual("Flower, Giraffe, Cabbage, Amusement, Texture, Bread", response.InstalledPackages);
        }

        [TestMethod]
        public void TestInstallPackagesInvalid_Medium()
        {
            string[] input = new string[]
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
            Assert.AreNotEqual(null, response);

            // This should report that it contains a cycle and no packages should be installed
            Assert.AreEqual(PackageInstallStatuses.CONTAINS_CYCLE, response.Status);
            Assert.AreEqual(string.Empty, response.InstalledPackages);
        }

        [TestMethod]
        public void TestGetPackageInfoList()
        {
            PackageInstallService service = new PackageInstallService();

            PrivateType privateType = new PrivateType(service.GetType());

            string[] input = new string[]
            {
                "KittenService: CamelCaser",
                "CamelCaser: "
            };

            object[] args = new object[1] { input };

            List<PackageInfo> packageInfoList = (List<PackageInfo>)privateType.InvokeStatic("GetPackageInfoList", args);

            // We should get a result
            Assert.AreNotEqual(null, packageInfoList);
            Assert.AreNotEqual(0, packageInfoList.Count);
            
            // Compare the first package
            Assert.AreEqual("KittenService", packageInfoList[0].Name);
            Assert.AreEqual("CamelCaser", packageInfoList[0].Dependency);

            // Compare the second package
            Assert.AreEqual("CamelCaser", packageInfoList[1].Name);
            Assert.AreEqual(string.Empty, packageInfoList[1].Dependency);
        }

        [TestMethod]
        public void TestGetInstalledPackagesFormatted()
        {
            PackageInstallService service = new PackageInstallService();

            PrivateType privateType = new PrivateType(service.GetType());

            string[] input = new string[]
            {
                "Texture: Cabbage",
                "Bread: Amusement",
                "Cabbage: Giraffe",
                "Amusement: Giraffe",
                "Giraffe: Flower",
                "Flower: "
            };

            object[] args = new object[1] { input };

            List<PackageInfo> packageInfoList = (List<PackageInfo>)privateType.InvokeStatic("GetPackageInfoList", args);

            object[] new_args = new object[1] { packageInfoList };

            string installedPackagesFormatted = (string)privateType.InvokeStatic("GetInstalledPackagesFormatted", new_args);

            // We should get a result
            Assert.AreNotEqual(string.Empty, installedPackagesFormatted);

            // We should get the following formatted order (will just be the order of the given list because we aren't testing the topological sort here
            Assert.AreEqual("Texture, Bread, Cabbage, Amusement, Giraffe, Flower", installedPackagesFormatted);
        }

        [TestMethod]
        public void TestGetCleanedPackageListFromInput_Easy()
        {
            string input = "[ \"KittenService: CamelCaser\", \"CamelCaser: \" ]";

            string[] packages = PackageInstallService.GetCleanedPackageListFromInput(input);

            // We should get a result
            Assert.AreNotEqual(null, packages);
            Assert.AreNotEqual(0, packages.Length);

            // We should get the following list
            Assert.AreEqual("KittenService: CamelCaser", packages[0]);
            Assert.AreEqual("CamelCaser: ", packages[1]);
        }
    }
}
