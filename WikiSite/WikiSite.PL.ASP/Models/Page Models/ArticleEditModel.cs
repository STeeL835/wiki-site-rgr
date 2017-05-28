using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WikiSite.BLL.Abstract;
using WikiSite.DI.Provider;
using WikiSite.Caretakers;

namespace WikiSite.PL.ASP.Models
{
    public class ArticleEditModel
    {
        private string _heading;
        private string _definition;
        private string _text;
        private static IArticlesBLL _bll;

        #region VM

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        [MinLength(5, ErrorMessage = "Минимальное количество символов - 5")]
        [MaxLength(50, ErrorMessage = "Минимальное количество символов - 50")]
        public string Heading
        {
            get { return _heading; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _heading = value;
            }
        }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(5, ErrorMessage = "Минимальное количество символов - 5")]
        [MaxLength(300, ErrorMessage = "Максимальное количество символов - 300")]
        [Display(Name = "Краткое описание")]
        public string Definition
        {
            get { return _definition; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _definition = value;
            }
        }

        [Required]
        [DataType(DataType.MultilineText)]
        [MinLength(5, ErrorMessage = "Минимальное количество символов - 5")]
        [Display(Name = "Содержание")]
        public string Text
        {
            get { return _text; }
            set
            {
                value = value.Trim();
                ErrorGuard.Check(value);
                _text = value;
            }
        }
        #endregion
        static ArticleEditModel()
        {
            _bll = Provider.ArticlesBLO;
        }

        public ArticleEditModel() { }

        public ArticleEditModel(ArticleVM article)
        {
            Heading = article.Heading;
            Definition = article.Definition;
            Text = article.Text;
        }

        /// <summary>
        /// Returns new and updated VM.
        /// Needs the original because id part gets lost from form,
        /// because it's never sent there
        /// </summary>
        /// <returns>New and updated VM.</returns>	
        public ArticleVM GetArticleVM(ArticleVM article)
        {
            article.Heading = Heading;
            article.Definition = Definition;
            article.Text = Text;
            return article;
        }
    }
}