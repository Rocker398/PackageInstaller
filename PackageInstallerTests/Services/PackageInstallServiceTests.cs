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
            Assert.AreNotEqual(response, null);

            // The response should be successful with this result
            Assert.AreEqual(response.Status, PackageInstallStatuses.SUCCESS);
            Assert.AreEqual(response.InstalledPackages, "CamelCaser, KittenService");
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
            Assert.AreNotEqual(response, null);

            // The response should be successful with this result
            Assert.AreEqual(response.Status, PackageInstallStatuses.SUCCESS);
            Assert.AreEqual(response.InstalledPackages, "KittenService, Ice, Cyberportal, Leetmeme, CamelCaser, Fraudstream");
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
            Assert.AreNotEqual(response, null);

            // This should report that it contains a cycle and no packages should be installed
            Assert.AreEqual(response.Status, PackageInstallStatuses.CONTAINS_CYCLE);
            Assert.AreEqual(response.InstalledPackages, string.Empty);
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
            Assert.AreNotEqual(packageInfoList, null);
            Assert.AreNotEqual(packageInfoList.Count, 0);
            
            // Compare the first package
            Assert.AreEqual(packageInfoList[0].Name, "KittenService");
            Assert.AreEqual(packageInfoList[0].Dependency, "CamelCaser");

            // Compare the second package
            Assert.AreEqual(packageInfoList[1].Name, "CamelCaser");
            Assert.AreEqual(packageInfoList[1].Dependency, string.Empty);
        }
    }
}
