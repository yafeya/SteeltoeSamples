﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Common.Net;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace SMBFileShares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string demoFileName = "TextFileToTransfer.txt";
        private readonly string sharePath;
        private NetworkCredential ShareCredentials { get; set; }

        public FilesController(IOptions<CloudFoundryServicesOptions> serviceOptions)
        {
            var credHubEntry = serviceOptions.Value.Services["credhub"].First(q => q.Name.Equals("steeltoe-network-share"));

            sharePath = credHubEntry.Credentials["location"].Value;
            string userName = credHubEntry.Credentials["username"].Value;
            string password = credHubEntry.Credentials["password"].Value;
            ShareCredentials = new NetworkCredential(userName, password);
        }

        // GET api/files
        [HttpGet("copy")]
        public ActionResult<string> Copy()
        {
            using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
            {
                System.IO.File.Copy(demoFileName, Path.Combine(sharePath, demoFileName), true);
            }

            return "File copied successfully";
        }

        [HttpGet("list")]
        public ActionResult<IEnumerable<string>> List()
        {
            using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
            {
                return new ActionResult<IEnumerable<string>>(Directory.EnumerateFiles(sharePath));
            }
        }

        [HttpGet("delete")]
        public ActionResult<string> Delete()
        {
            using (WindowsNetworkFileShare networkPath = new WindowsNetworkFileShare(sharePath, ShareCredentials))
            {
                System.IO.File.Delete(Path.Combine(sharePath, demoFileName));
            }

            return "File deleted successfully";
        }
    }
}
