package com.swd392.group7.phygen.Qdrant;

import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.MediaType;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.List;
import java.util.Map;

@Service
@RequiredArgsConstructor
public class QdrantService {

    @Value("${spring.ai.vectorstore.qdrant.uri}")
    private String host;

    private final String collectionName = "SWD392_PhyGen";

    private static final int VECTOR_SIZE = 1024;

    @Value("${qdrant_api_key}")
    private String QDRANT_API_KEY;

    private HttpEntity<Map<String, Object>> buildEntity(Map<String, Object> body) {
        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_JSON);
        headers.set("api-key", QDRANT_API_KEY);

        return new HttpEntity<>(body, headers);
    }

    public void createCollection(String name) {
        Map<String, Object> body = Map.of(
                "vectors", Map.of(
                        "size", VECTOR_SIZE,
                        "distance", "Cosine"
                )
        );
        HttpEntity<Map<String, Object>> request = buildEntity(body);
        new RestTemplate().put(host + "/collections/" + name, request);
    }

    public void uploadDataCollection(List<PointDto> points){
        Map<String, Object> body = Map.of("points",points);
        HttpEntity<Map<String,Object>> request = buildEntity(body);
        new RestTemplate().put(host + "/collections/" + collectionName,request);
    }

    public void searchDataCollection(SearchDto searchDto){
        Map<String, Object> body = Map.of(
                "vector", searchDto.getVector(),
                "limit", searchDto.getLimit(),
                "with_payload", searchDto.isWith_payload()
        );
        HttpEntity<Map<String,Object>> request = buildEntity(body);
        new RestTemplate().postForEntity(host + "/collections/"+ collectionName + "/points/search",request,String.class);

    }





}
