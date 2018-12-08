using System.Collections.Generic;
using SME.Models;

namespace SME.Services
{
    public interface IDatabaseCommunicator
    {
        QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest);
        List<Resource> ProvideRecommendedResources(List<string> resourceIds);
        void HandleQuestionRequestFromQueue();
        void HandleResourceRequestFromQueue();
    }
}