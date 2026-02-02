import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function middleware(req: NextRequest) {
  const token = req.cookies.get("token")?.value;

  if (!token && req.nextUrl.pathname.startsWith("/dashboard")) {
    return NextResponse.redirect(new URL("auth/login", req.url));
    }
// continue to next middleware /route
  return NextResponse.next();
}
//configeration for path on which the misddleware excuted
export const config = {
  matcher: ["/dashboard/:path*"],
};
