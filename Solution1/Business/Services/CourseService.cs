using System;
using System.Collections.Generic;
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
        private readonly IPlayerCourseRepository _playerCourseRepository;
        private const string DefaultCourseAvatarPath = "defaultCourse.png";

        public CourseService(ICourseRepository courseRepository, IPlayerService playerService, IPlayerCourseRepository playerCourseRepository)
        {
            _courseRepository = courseRepository;
            _playerService = playerService;
            _playerCourseRepository = playerCourseRepository;
        }

        public IEnumerable<Course> GetAllCourses(bool includeModules = false)
        {
            return includeModules ? _courseRepository.GetAllWithModules() : _courseRepository.GetAll().ToList();
        }

        public IEnumerable<Course> GetCoursesForPlayer(string playerId, bool includeModules = false)
        {
            var courses = _playerService.GetPlayerData(playerId, true, true).Courses;
            return courses;
        }

        public IEnumerable<Course> GetCoursesForAuthor(string authorName)
        {
            var courses = _courseRepository.GetCoursesByAuthor(authorName);
            return courses;
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

                courseFromData = _courseRepository.Create(courseFromData);
                var playerCourse = new PlayerCourse()
                {
                    CourseId = courseFromData.Id,
                    PlayerId = me.Id
                };
                playerCourse = _playerCourseRepository.Create(playerCourse);

                // step 2 - upload photo
                IUpload uploader = new Upload(new FileDataSource());
                var success = Upload(uploader, files, courseFromData.Id.ToString());
                if (!success)
                {
                    _playerCourseRepository.Delete(playerCourse);
                    _courseRepository.Delete(courseFromData);
                    throw new Exception();
                }
                
                // finally set the image
                courseFromData.PhotoUrl = files[0].FileName;
                _courseRepository.Update(courseFromData);
                return courseFromData;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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

        public void Partikip(string userId, string courseId)
        {
            var me =_playerService.GetPlayerData(userId, true);
            Guid courseGid;
            Guid.TryParse(courseId, out courseGid);
            var course = _courseRepository.GetById(courseGid);

            var playerCourse = _playerCourseRepository.GetByPlayerAndCourse(me.Id, course.Id);

            if (playerCourse != null)
                throw new Exception("Already subscribed to the course.");

            try
            {
                playerCourse = new PlayerCourse()
                {
                    CourseId = course.Id,
                    PlayerId = me.Id
                };
                _playerCourseRepository.Create(playerCourse);
            }
            catch
            {
                throw new Exception("Cannot subscribe to the course.");
            }
            
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

