import { AuthDetailRoleDto } from './auth-detail-role-dto.model';

export class AuthDetailDto {
    public userId: string;
    public accessToken: string;
    public role: AuthDetailRoleDto;
    public errorMessage: string;
}
