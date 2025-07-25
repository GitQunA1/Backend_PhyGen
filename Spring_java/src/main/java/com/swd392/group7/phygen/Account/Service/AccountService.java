package com.swd392.group7.phygen.Account.Service;

import com.swd392.group7.phygen.Account.Entity.Account;
import com.swd392.group7.phygen.Account.Exception.AccountException;
import com.swd392.group7.phygen.Account.Repository.AccountRepo;
import com.swd392.group7.phygen.Authen.Jwt.JwtProvider;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

@Service
@RequiredArgsConstructor
public class AccountService {

    private final AccountRepo accountRepo;

    private final JwtProvider jwtProvider;

    public Account getAccountByToken(String token) {
        Long id = jwtProvider.extractAccountId(token);
        return findAccountById(id);
    }

    public Account findAccountById(Long id){
        return accountRepo.findById(id).orElseThrow(()->new AccountException("Account not found", HttpStatus.NOT_FOUND));
    }

    public Account getCurrentAccount(){
        Account account = (Account) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
        return findAccountById(account.getId());
    }

}
