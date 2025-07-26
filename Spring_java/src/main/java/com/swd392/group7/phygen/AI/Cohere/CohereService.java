package com.swd392.group7.phygen.AI.Cohere;

import com.cohere.api.Cohere;
import com.cohere.api.resources.v2.requests.V2EmbedRequest;
import com.cohere.api.types.EmbedByTypeResponse;
import com.cohere.api.types.EmbedInputType;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
public class CohereService {

    private final Cohere cohere;

    public List<List<Double>> embedDocument(String text){
        EmbedByTypeResponse response = cohere.v2()
                .embed(
                        V2EmbedRequest
                                .builder()
                                .model("embed-multilingual-v3.0")
                                .inputType(EmbedInputType.SEARCH_DOCUMENT)
                                .texts(List.of(text))
                                .build()
                );
        return response.getEmbeddings().getFloat().orElseThrow(()->new CohereException("Cant find", HttpStatus.NOT_FOUND));
    }

    public List<List<Double>> embedQuery(String text){
        EmbedByTypeResponse response = cohere.v2()
                .embed(
                        V2EmbedRequest
                                .builder()
                                .model("embed-multilingual-v3.0")
                                .inputType(EmbedInputType.SEARCH_QUERY)
                                .texts(List.of(text))
                                .build()
                );
        return response.getEmbeddings().getFloat().orElseThrow(()->new CohereException("Cant find", HttpStatus.NOT_FOUND));
    }

}
