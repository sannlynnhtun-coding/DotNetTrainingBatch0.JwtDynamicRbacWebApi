# JWT Dynamic RBAC Web API

## Project Overview
This project implements **JWT Authentication** with **Dynamic Role-Based Access Control (RBAC)**. Unlike static RBAC, this system manages Roles and Permissions through a relational database structure (`Users -> Roles -> RolePermissions -> Permissions`). This allows administrators to modify permissions for a role in the database without changing any code or restarting the application.

## Step-by-Step Flow

### 1. Authentication (Login)
- The client sends credentials to `api/auth/login`.
- `AuthService` finds the user and identifies their `RoleId`.

### 2. Dynamic Permission Fetching
- The server performs a join query across `TblRoles`, `TblRolePermissions`, and `TblPermissions` to fetch all active permissions associated with the user's role at the moment of login.
- This ensures that any changes made to the role-permission mapping in the database are reflected the next time a user logs in.

### 3. Token Generation
- A JWT is created containing the user's identity, their assigned role name, and the list of permissions as custom claims.
- The token is sent back to the client.

### 4. Middleware Validation
- For every request, the `JwtBearer` middleware extracts the claims from the token.

### 5. Authorization
- Access is controlled via `[Authorize]` attributes. 
- Since permissions are included as claims in the token, the application can use policy-based authorization to check if the required permission claim exists.

## Database Schema
- **TblUsers**: Stores user identity and foreign key to `TblRoles`.
- **TblRoles**: Defines user roles (e.g., Admin, Staff, Editor).
- **TblPermissions**: Defines granular actions (e.g., `Product.Create`, `User.Delete`).
- **TblRolePermissions**: A mapping table that links Roles to multiple Permissions.
