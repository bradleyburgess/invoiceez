# Changelog

All notable changes to InvoiceEZ will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.1] - 2025/11/01

### Fixed
- culture invariance bug

## [0.2.0] - 2025-10-26

### Added
- Feature: Duplicate invoice items and discounts
- Feature: Duplicate invoice

### Changed
- Light refactoring

## [0.1.3] - 2025-10-26

### Added
- Version number in footer

### Changed
- Invoice list now in table format with date ordering and filtering

## [0.1.2] - 2025-10-24

### Fixed
- Environment variable injection

## [0.1.1] - 2025-10-24

### Fixed
- GitHub Actions workflow `dockerfile` locations

## [0.1.0] - 2025-10-24

### Added
- Initial project setup with Nuxt.js frontend and .NET backend structure
- Basic invoice management system
  - CRUD operations for `Business`, `Invoice` and `Customer`
  - Generating PDF invoices
- User authentication and authorization
- Database schema and migrations
- API endpoints for CRUD operations
- Frontend components and routing
- Development environment configuration
- Core dependencies installation
- Basic documentation

### Security
- Basic security measures implemented
- Authentication middleware
- Input validation
