using System.ComponentModel;

namespace PANDA.Shared.Intents
{
    /// <summary>
    /// Helper for building OpenAI system prompt and working with intent metadata.
    /// </summary>
    public static class ChatbotIntentHelper
    {
        /// <summary>
        /// Builds a consistent system prompt for the OpenAI classifier.
        /// </summary>
        public static string BuildSystemPrompt()
        {
            var lines = new List<string>
            {
                "You are an intent classifier for a healthcare assistant chatbot.",
                "",
                "Possible intents:"
            };

            foreach (var intent in Enum.GetValues(typeof(ChatbotIntent)).Cast<ChatbotIntent>())
            {
                var desc = intent.GetDescription();
                lines.Add($"- {intent}: {desc}");
            }

            lines.Add("");
            lines.Add("Respond ONLY with the intent name EXACTLY as shown above. Do not add any other words or explanations.");

            return string.Join(Environment.NewLine, lines);
        }

        /// <summary>
        /// Gets the description for an enum value.
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attr?.Description ?? value.ToString();
        }
    }
}