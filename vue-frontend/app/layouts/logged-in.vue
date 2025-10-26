<script setup lang="ts">
import { LogOut, Moon, Sun } from "lucide-vue-next"
import { getUserDisplayName, getUserInitials } from "~/lib/utils";
import { navigationMenuTriggerStyle } from "@/components/ui/navigation-menu";

const colorMode = useColorMode()

const auth = useAuth();
const user = auth.getCurrentUser();
const displayName = getUserDisplayName(user!);
const initials = getUserInitials(user!);

async function onLogout() {
    await auth.logout();
    navigateTo('/auth/login');
}

function onThemeChange() {
    colorMode.preference = colorMode.value === 'dark' ? 'light' : 'dark';
}
</script>

<template>
    <div class="h-full flex flex-col">
        <header>
            <div class="container mx-auto">
                <div class="flex justify-between items-center p-2">
                    <div>
                        <NuxtLink to="/" class="flex items-center max-w-28">
                            <InvoiceezLogo />
                        </NuxtLink>
                    </div>
                    <div class="flex gap-2">
                        <NavigationMenu>
                            <NavigationMenuList>
                                <NavigationMenuItem>
                                    <NuxtLink v-slot="{ isActive, href, navigate }" to="/businesses" custom>
                                        <NavigationMenuLink :active="isActive" :href
                                            :class="navigationMenuTriggerStyle()" @click="navigate">
                                            Businesses
                                        </NavigationMenuLink>
                                    </NuxtLink>
                                </NavigationMenuItem>
                                <NavigationMenuItem>
                                    <NuxtLink v-slot="{ isActive, href, navigate }" to="/invoices" custom>
                                        <NavigationMenuLink :active="isActive" :href
                                            :class="navigationMenuTriggerStyle()" @click="navigate">
                                            Invoices
                                        </NavigationMenuLink>
                                    </NuxtLink>
                                </NavigationMenuItem>
                            </NavigationMenuList>
                        </NavigationMenu>
                        <DropdownMenu>
                            <DropdownMenuTrigger>
                                <div class="cursor-pointer">
                                    <Avatar>
                                        <AvatarFallback>{{ initials }}</AvatarFallback>
                                    </Avatar>
                                </div>
                            </DropdownMenuTrigger>
                            <DropdownMenuContent align="end" class="w-56">
                                <DropdownMenuLabel class="font-normal">
                                    <div class="flex flex-col space-y-1">
                                        <p class="text-sm font-medium leading-none">Logged in as {{ displayName }}</p>
                                    </div>
                                </DropdownMenuLabel>
                                <DropdownMenuSeparator />
                                <DropdownMenuItem @click="onThemeChange">
                                    <Sun v-if="colorMode.value === 'dark'" class="mr-2 h-4 w-4" />
                                    <Moon v-else class="mr-2 h-4 w-4" />
                                    <span v-if="colorMode.value === 'dark'">Light Mode</span>
                                    <span v-else>Dark Mode</span>
                                </DropdownMenuItem>
                                <DropdownMenuSeparator />
                                <DropdownMenuItem @click="onLogout">
                                    <LogOut class="mr-2 h-4 w-4" />
                                    <span>Log out</span>
                                </DropdownMenuItem>
                            </DropdownMenuContent>
                        </DropdownMenu>
                    </div>
                </div>
            </div>
        </header>
        <main class="flex-grow-1 px-2 pb-16">
            <div class="h-full mt-10">
                <slot />
            </div>
        </main>
        <AppFooter />
    </div>
</template>