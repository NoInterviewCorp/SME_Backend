using SME.Models;

namespace SME.Services{
    public interface IQuestionRequestHandler
    {
        QuestionBatchResponse ProvideQuestionsFromId(QuestionBatchRequest batchRequest);
        void HandleQuestionRequestFromQueue();
    }
}