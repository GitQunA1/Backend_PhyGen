package com.swd392.group7.phygen.Authen.Jwt;

import com.swd392.group7.phygen.Account.Entity.Account;
import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.io.Decoders;
import io.jsonwebtoken.security.Keys;
import jakarta.annotation.PostConstruct;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

import javax.crypto.SecretKey;
import java.nio.charset.StandardCharsets;
import java.util.Date;

@Component
public class JwtProvider {

    @Value("${secret_key}")
    private String KEY_SECRET;

    @PostConstruct
    public void init() {
        System.out.println("KEY_SECRET = " + KEY_SECRET);
    }


    private SecretKey getSignKey() {
        //dung kieu raw text
        byte[] keyBytes = KEY_SECRET.getBytes(StandardCharsets.UTF_8);
        return Keys.hmacShaKeyFor(keyBytes);
    }

    public String generateToken(Account account){
        return Jwts.builder()
                .subject(account.getId() + "")
                .issuedAt(new Date(System.currentTimeMillis()))
                .expiration(new Date(System.currentTimeMillis() + 1000*60*60*24))
                .signWith(getSignKey())
                .compact();
    }

    public Claims parseToken(String token) {
        return Jwts.parser()
                .verifyWith(getSignKey())
                .build()
                .parseSignedClaims(token)
                .getPayload();
    }

    public Long extractAccountId(String token) {
        return Long.parseLong(parseToken(token).getSubject().trim());
    }
}
