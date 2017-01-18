using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Business.CommonInfrastructure;
using Business.CommonInfrastructure.Interfaces;
using Business.Services.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Business.Services
{

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IPlayerService _playerService;
        private const string DefaultCourseAvatarPath = "defaultCourse.png";

        public CourseService(ICourseRepository courseRepository, IPlayerService playerService)
        {
            _courseRepository = courseRepository;
            _playerService = playerService;
        }

        public IEnumerable<Course> GetAllCourses(bool includeModules = false)
        {
            return includeModules ? _courseRepository.GetAllWithModules() : _courseRepository.GetAll().ToList();
        }

        public IEnumerable<string> GetAllCourseNames()
        {
            return _courseRepository.GetCourseNames();
        }

        public Course CreateCourse(string userid, string title, string description, string hashtag, string datalink, IList<IFormFile> files)
        {
            try
            {
                // step 1 - create course
                var me = _playerService.GetPlayerData(userid);
                var courseFromData = new Course()
                {
                    Title = title,
                    Description = description,
                    Author = me.FirstName + " " + me.LastName,
                    TimeStamp = DateTime.Now
                };
                if (datalink != null) courseFromData.DataLink = datalink;
                if (hashtag != null)
                {
                    if (hashtag.StartsWith("#") && hashtag.Length != 1)
                        hashtag = hashtag.Substring(1);
                    courseFromData.HashTag = hashtag;
                }
                
                me.Courses.Add(courseFromData);
                _playerService.UpdatePlayer(me);

                // step 2 - upload photo
                IUpload uploader = new Upload(new FileDataSource());
                var success = Upload(uploader, files, courseFromData.Id.ToString());
                if (!success)
                {
                    me.Courses.Remove(courseFromData);
                    _playerService.UpdatePlayer(me);
                    throw new Exception();
                }
                
                // finally set the image
                courseFromData.PhotoUrl = files[0].FileName;
                _courseRepository.Update(courseFromData);
                return courseFromData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool Upload(IUpload upload, IList<IFormFile> files, string courseId)
        {
            if (upload == null || courseId == null)
                return false;
            
            var root = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Avatars", "Courses", courseId);

            var uploadedPaths = upload.UploadFiles(files, root);
            return uploadedPaths.Count == files.Count;
        }

        public string GetImagePathForCourseId(string id)
        {
            Guid courseId;
            Guid.TryParse(id, out courseId);
            var course = _courseRepository.GetById(courseId);
            if (course == null) return null;

            var root = Path.Combine(Directory.GetCurrentDirectory(), @"Data\Avatars\Courses\");
            var path = root + id + @"\" + course.PhotoUrl;
            if (!File.Exists(path))
                path = root + DefaultCourseAvatarPath;
            return path;
        }

        public void UpdateCourse(Course course)
        {
            _courseRepository.Update(course);
        }

        public void DeleteCourse(Course course)
        {
            _courseRepository.Delete(course);
        }
    }
}

