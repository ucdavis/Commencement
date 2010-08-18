﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Commencement.Helpers;
using Telerik.Web.Mvc.UI;

namespace Commencement.Controllers.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static CustomGridBuilder<T> Grid<T>(this HtmlHelper htmlHelper, IEnumerable<T> dataModel) where T : class
        {
            var builder = htmlHelper.Telerik().Grid(dataModel);

            return new CustomGridBuilder<T>(builder);
        }

        private const string HtmlTag = @"&lt;{0}&gt;";
        private const string Span = "span";
        private const string SpanEncodedStyled = @"&lt;span style=&quot;{0}&quot;&gt;";
        private const string SpanStyled = @"<span style=""{0}"">";

        private const string Underline = "text-decoration: underline;";
        private const string XXSmallText = "font-size: xx-small;";
        private const string XSmallText = "font-size: x-small;";
        private const string SmallText = "font-size: small;";
        private const string MediumText = "font-size: medium;";
        private const string LargeText = "font-size: large;";

        /// <summary>
        /// This allows limited html encoding, while still encoding the rest of the string
        /// </summary>
        /// <remarks>
        /// The selected tags are what have been allowed with the TinyMce Text Editor
        /// 
        /// Allows:
        ///     <p></p>
        ///     <strong></strong>
        ///     <em></em>
        ///     <span style="text-decoration:underline;"></span>    // based on what is generated by TinyMce
        ///     <ul></ul>
        ///     <ol></ol>
        ///     <li></li>
        ///     <h1></h1>
        ///     &nbsp;
        ///     <address></address>
        /// </remarks>
        /// <param name="helper"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlEncode(this HtmlHelper helper, string text)
        {
            // encode the string
            string encodedText = HttpUtility.HtmlEncode(text);

            // put the text in a string builder
            StringBuilder formattedEncodedText = new StringBuilder(encodedText);

            // replace the escaped characters with the correct strings to allow formatting
            ReplaceTagContents(formattedEncodedText, "p");
            ReplaceTagContents(formattedEncodedText, "strong");
            ReplaceTagContents(formattedEncodedText, "em");
            ReplaceTagContents(formattedEncodedText, "ul");
            ReplaceTagContents(formattedEncodedText, "ol");
            ReplaceTagContents(formattedEncodedText, "li");
            ReplaceTagContents(formattedEncodedText, "address");
            ReplaceTagContents(formattedEncodedText, "h1");
            ReplaceTagContents(formattedEncodedText, "h2");
            ReplaceTagContents(formattedEncodedText, "h3");
            ReplaceTagContents(formattedEncodedText, "h4");
            ReplaceTagContents(formattedEncodedText, "h5");
            ReplaceTagContents(formattedEncodedText, "h6");
            ReplaceSingleTagContents(formattedEncodedText, "br");

            // replace &nbsp;
            formattedEncodedText = formattedEncodedText.Replace(@"&amp;nbsp;", @"&nbsp;");

            // <span style="text-decoration:underline;">
            ReplaceComplexTag(formattedEncodedText, Span,
                              string.Format(SpanEncodedStyled, Underline),
                              string.Format(SpanStyled, Underline));

            // <span style="font-size: xx-small;">
            ReplaceComplexTag(formattedEncodedText, Span, 
                              string.Format(SpanEncodedStyled, XXSmallText),
                              string.Format(SpanStyled, XXSmallText));

            // <span style="font-size: x-small;">
            ReplaceComplexTag(formattedEncodedText, Span,
                              string.Format(SpanEncodedStyled, XSmallText),
                              string.Format(SpanStyled, XSmallText));

            // <span style="font-size: small;">
            ReplaceComplexTag(formattedEncodedText, Span,
                              string.Format(SpanEncodedStyled, SmallText),
                              string.Format(SpanStyled, SmallText));

            // <span style="font-size: medium;">
            ReplaceComplexTag(formattedEncodedText, Span,
                              string.Format(SpanEncodedStyled, MediumText),
                              string.Format(SpanStyled, MediumText));

            // <span style="font-size: large;">
            ReplaceComplexTag(formattedEncodedText, Span,
                              string.Format(SpanEncodedStyled, LargeText),
                              string.Format(SpanStyled, LargeText));

            return formattedEncodedText.ToString();
        }

        public static void ReplaceTagContents(StringBuilder formattedText, string tag)
        {
            // opening tag
            formattedText.Replace(string.Format(HtmlTag, tag), @"<" + tag + ">");
            // closing tag
            formattedText.Replace(string.Format(HtmlTag, @"/" + tag), @"</" + tag + ">");
        }
        public static void ReplaceSingleTagContents(StringBuilder formattedText, string tag)
        {
            // opening tag
            formattedText.Replace(string.Format(HtmlTag, tag + @" /"), @"<" + tag + @" />");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formattedText">The formatted text string for final output</param>
        /// <param name="tag">Tag name we are searching for, only the tag name</param>
        /// <param name="searchTag">Tag we are searching for, with all properties included (html encoded)</param>
        /// <param name="replacementTag">Formatted tag to be replaced in so it shows correctly.</param>
        public static void ReplaceComplexTag(StringBuilder formattedText, string tag, string searchTag, string replacementTag)
        {
            var closingTag = string.Format(HtmlTag, @"/" + tag);

            // replace html encoded tag with the correct formatted tag
            formattedText.Replace(searchTag, replacementTag);
            
            // determine information needed to replace correctly
            var map = MapOpeningTags(formattedText.ToString(), replacementTag, tag, closingTag);
            var stack = CalculateStack(map);

            // process and replace closing tags
            while(stack.Count > 0)
            {
                var tm = stack.Pop();

                var tmpFormattedText = formattedText.ToString();

                // get the location of the nth opening tag
                var openIndex = FindNthIndex(tmpFormattedText, replacementTag, 0, tm.Count);

                // get the location of the matching closing tag
                var closingIndex = FindNthIndex(tmpFormattedText, closingTag, openIndex, tm.OffSet);

                // replace the tag

                formattedText.Remove(closingIndex, closingTag.Length);
                formattedText.Insert(closingIndex, @"</" + tag + ">");
            } 
        }

        public static Stack<TagMarker> CalculateStack(string[] map)
        {
            var stack = new Stack<TagMarker>();
            var count = 1;

            for (int i = 0; i < map.Length; i++ )
            {
                // found a valid opening tag, push a new marker on to the stack
                if (map[i] == "o")
                {
                    stack.Push(new TagMarker(count));
                    count++;
                }
                // found a misc open tag that we shouldn't replace, update the stack to reflect the offset
                else if (map[i] == "m")
                {
                    // go top down and add offset until we come accros a completed marker
                    foreach(var tm in stack.Reverse())
                    {
                        // completed marker, cheese it!
                        if (tm.Completed) break;
                        
                        // increase the offset count
                        tm.OffSet++;
                    }
                }
                else if (map[i] == "c")
                {
                    // go top down to find the highest not completed object, then mark completed
                    foreach(var tm in stack.Reverse())
                    {
                        if (!tm.Completed)
                        {
                            // increment the misc closing found
                            tm.MiscClosingFound++;

                            // once misc closing found > offset, mark completed and we can exit the loop, otherwise we just go down until we hit bottom
                            //  or find one that meets the criteria
                            if (tm.MiscClosingFound > tm.OffSet )
                            {
                                tm.Completed = true;
                                break;
                            }
                        }
                    }
                }
            }

            return stack;
        }

        /// <summary>
        /// Finds the locations of all opening tags and misc tags with matching tag name
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="replacementTag"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string[] MapOpeningTags(string searchText, string replacementTag, string tag, string closingTag)
        {
            var openingTags = searchText.IndexOfAll(replacementTag);
            var miscOpeningTag = searchText.IndexOfAll(@"&lt;" + tag);
            var closingTags = searchText.IndexOfAll(closingTag);

            var map = new string[searchText.Length];
            foreach (int a in miscOpeningTag) map[a] = "m";
            foreach (int a in openingTags) map[a] = "o";
            foreach (int a in closingTags) map[a] = "c";

            return map;
        }

        /// <summary>
        /// Finds the nth instance of a search string
        /// </summary>
        /// <param name="searchText">String we are searching</param>
        /// <param name="searchString">String we are searching for in the searchText</param>
        /// <param name="startLocation">Start location</param>
        /// <param name="offset">Nth instance to find</param>
        /// <returns></returns>
        public static int FindNthIndex(string searchText, string searchString, int startLocation, int offset)
        {
            var offsetCounter = 0;
            var currentLocation = startLocation;

            do
            {
                currentLocation = searchText.IndexOf(searchString, currentLocation);
                offsetCounter++;
            } while (offsetCounter < offset);

            return currentLocation;
        }
    }

    public class TagMarker
    {
        public TagMarker(int count)
        {
            Count = count;

            Completed = false;
            OffSet = 0;
            MiscClosingFound = 0;
        }

        public int Count { get; set; }
        public int OffSet { get; set; } // # of closing tags to skip before replacing
        
        // used during discovery algorithm
        public bool Completed { get; set; }         // signifies if we discovered the closing one or not
        // used during replacement
        public int MiscClosingFound { get; set; }   // # of closing tags skipped so far, when this matches offset we found the closing tag for this, so skip that
    }
}
