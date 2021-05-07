import Response from '@/models/cqrs/Response';

interface ServiceUnreachableResponse extends Response { };

const serviceUnreachableResponse: ServiceUnreachableResponse = {
    errors: ['Service unreachable.'],
    success: false
};
export default serviceUnreachableResponse;
