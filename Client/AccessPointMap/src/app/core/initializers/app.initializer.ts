import { AuthService } from "src/app/authentication/services/auth.service";

export function appInitializer(authService: AuthService) {
    return () => new Promise(resolve => {
        authService.refreshToken()
            .subscribe().add(resolve);
    });
}