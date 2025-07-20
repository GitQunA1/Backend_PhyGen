package com.swd392.group7.phygen.Config;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Service.AccountService;
import io.jsonwebtoken.ExpiredJwtException;
import io.jsonwebtoken.MalformedJwtException;
import jakarta.security.auth.message.AuthException;
import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.NonNull;
import lombok.RequiredArgsConstructor;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.security.web.authentication.WebAuthenticationDetailsSource;
import org.springframework.stereotype.Component;
import org.springframework.util.AntPathMatcher;
import org.springframework.web.filter.OncePerRequestFilter;
import org.springframework.web.servlet.HandlerExceptionResolver;

import java.io.IOException;
import java.util.List;

@Component
public class Filter extends OncePerRequestFilter {

    @Autowired
    AccountService accountService;

    @Autowired
    @Qualifier("handlerExceptionResolver")
    HandlerExceptionResolver handlerExceptionResolver;


    private final List<String> AUTH_PERMISSION = List.of(
            "/swagger-ui/**",
            "/v3/api-docs/**",
            "/swagger-resources/**",
            "/api/Gemini/**",
            "/api/OCR/**",
            "/api/Qdrant/**",
            "/api/exams/structure",
            "/api/topics/**"
    );


    private String getToken(HttpServletRequest request){
        String authHeader = request.getHeader("Authorization");
        if(authHeader==null) return null;
        return authHeader.substring(7);
    }

    private boolean isPublicAPI(String uri){
        AntPathMatcher pathMatcher = new AntPathMatcher();
        return AUTH_PERMISSION.stream().anyMatch(pattern -> pathMatcher.match(pattern,uri));
    }

    @Override
    protected void doFilterInternal(HttpServletRequest request, @NonNull HttpServletResponse response,@NonNull FilterChain filterChain) throws ServletException, IOException {
        boolean isPublicAPI = isPublicAPI(request.getRequestURI());
        if(isPublicAPI){
            filterChain.doFilter(request, response);
        }else {
            String token = getToken(request);
            if (token == null) {
                handlerExceptionResolver.resolveException(request, response, null, new AuthException("Empty token"));
                return;
            }
            Account account;
            try {
                account = accountService.getAccountByToken(token);
            }catch(ExpiredJwtException e){
                handlerExceptionResolver.resolveException(request, response,null,new AuthException("Token expired"));
                return;
            }catch(MalformedJwtException malformedJwtException){
                handlerExceptionResolver.resolveException(request,response,null,new AuthException("Token invalid"));
                return;
            }catch(Exception e){
                e.printStackTrace();
                return;
            }
            UsernamePasswordAuthenticationToken authenticationToken = new UsernamePasswordAuthenticationToken(account,token,account.getAuthorities());
            authenticationToken.setDetails(new WebAuthenticationDetailsSource().buildDetails(request));
            SecurityContextHolder.getContext().setAuthentication(authenticationToken);
            filterChain.doFilter(request,response);
        }
    }
}
